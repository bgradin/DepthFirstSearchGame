using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameClassLibrary
{
	public enum PostEffect
	{
		None,
		Only_BlurLow,
		Only_BlurMid,
		Only_BlurHigh,
		LinearAttenuation,
		LinearAttenuation_BlurLow,
		LinearAttenuation_BlurMid,
		LinearAttenuation_BlurHigh,
		CurveAttenuation,
		CurveAttenuation_BlurLow,
		CurveAttenuation_BlurMid,
		CurveAttenuation_BlurHigh
	}

	public sealed class ShadowMapResolver : IDisposable
	{
		public const SurfaceFormat SurfaceLight = SurfaceFormat.Color;

		public const int ReductionPower = 8;
		private static QuadRenderComponent QuadRender = new QuadRenderComponent();
		private GraphicsDevice graphicsDevice;

		int resolverRadiusDesired;
		int lightRadiusEffective;
		int shadowMapResolverSize;

		LightsFX lightsFX;

		RenderTarget2D shadowMapDistorted;
		RenderTarget2D shadowMapDigested;
		RenderTarget2D shadowsRT;
		RenderTarget2D processedShadowsRT;
		RenderTarget2D distancesRT;

		Vector2 currenteRenderTargetSize = Vector2.Zero;

		ShadowMapReductionStep[] reductionSteps;
		private int effectiveResolverMapSize;

		private float distanceMod = 1f;

		public ShadowMapResolver(GraphicsDevice graphicsDevice, LightsFX lightsFX, int resolverRadius)
		{
			this.graphicsDevice = graphicsDevice;

			this.lightsFX = lightsFX;

			this.resolverRadiusDesired = resolverRadius;

			int resolverRadiusModified = this.resolverRadiusDesired;

			bool reductionPlanCreated = false;
			List<ShadowMapReductionStep> listReductionSteps = new List<ShadowMapReductionStep>();
			int emergencyBreak = 15;
			while (reductionPlanCreated == false)
			{
				if (reductionPlanCreated)
					break;

				this.effectiveResolverMapSize = resolverRadiusModified * 2;
				listReductionSteps.Clear();
				int resolverMapSizeToReduce = this.effectiveResolverMapSize;

				for (int i = 0; i < 10; i++)
				{
					bool reductionStepFound = false;
					for (int ii = ShadowMapResolver.ReductionPower; ii > 1; ii--)
					{
						if (this.CheckReductionStep(ref resolverMapSizeToReduce, ii, listReductionSteps))
						{
							reductionStepFound = true;
							break;
						}
					}

					if (reductionStepFound == false)
						break;

					if (resolverMapSizeToReduce == 2)
					{
						reductionPlanCreated = true;
						break;
					}
				}
				emergencyBreak--;
				resolverRadiusModified++;
				if (emergencyBreak == 0 && reductionPlanCreated == false)
					throw new Exception("It has been impossible to create a shadow map resolver with radius " + resolverRadius);
			}
			this.reductionSteps = listReductionSteps.ToArray();
			this.lightRadiusEffective = resolverRadiusModified;
			this.shadowMapResolverSize = this.lightRadiusEffective * 2;

			shadowMapDistorted = new RenderTarget2D(graphicsDevice, this.shadowMapResolverSize, this.shadowMapResolverSize, false, ShadowMapResolver.SurfaceLight, DepthFormat.None);
			distancesRT = new RenderTarget2D(graphicsDevice, this.shadowMapResolverSize, this.shadowMapResolverSize, false, ShadowMapResolver.SurfaceLight, DepthFormat.None);
			shadowMapDigested = new RenderTarget2D(graphicsDevice, 2, this.shadowMapResolverSize, false, ShadowMapResolver.SurfaceLight, DepthFormat.None);
			shadowsRT = new RenderTarget2D(graphicsDevice, this.shadowMapResolverSize, this.shadowMapResolverSize, false, ShadowMapResolver.SurfaceLight, DepthFormat.None);
			processedShadowsRT = new RenderTarget2D(graphicsDevice, this.shadowMapResolverSize, this.shadowMapResolverSize, false, ShadowMapResolver.SurfaceLight, DepthFormat.None);
		}

		private bool CheckReductionStep(ref int resolverSizeToReduce, int stepSize, List<ShadowMapReductionStep> listReductionSteps)
		{
			if (((float)resolverSizeToReduce) % ((float)stepSize) == 0 && ((float)(resolverSizeToReduce / stepSize)) % 2f == 0 && resolverSizeToReduce / stepSize >= 2)
			{
				if (resolverSizeToReduce / stepSize == 2)
					listReductionSteps.Add(new ShadowMapReductionStep(stepSize, null));
				else
					listReductionSteps.Add(new ShadowMapReductionStep(stepSize, new RenderTarget2D(graphicsDevice, resolverSizeToReduce / stepSize, effectiveResolverMapSize, false, ShadowMapResolver.SurfaceLight, DepthFormat.None)));

				resolverSizeToReduce = resolverSizeToReduce / stepSize;

				return true;
			}

			return false;
		}

		public void ResolveShadows(ShadowCasterMap shadowCasterMap, LightSource resultLight, PostEffect postEffect, Vector2 newPosition, int radius)
		{
			resultLight.Position = newPosition;
			this.ResolveShadows(shadowCasterMap, resultLight, postEffect, 1f, radius);
		}

		public void ResolveShadows(ShadowCasterMap shadowCasterMap, LightSource resultLight, PostEffect postEffect, float distanceMod, Vector2 newPosition, int radius)
		{
			resultLight.Position = newPosition;
			this.ResolveShadows(shadowCasterMap, resultLight, postEffect, distanceMod, radius);
		}

		public void ResolveShadows(ShadowCasterMap shadowCasterMap, LightSource resultLight, PostEffect postEffect, int radius)
		{
			this.ResolveShadows(shadowCasterMap, resultLight, postEffect, 1f, radius);
		}

		public void ResolveShadows(ShadowCasterMap shadowCasterMap, LightSource resultLight, PostEffect postEffect, float distanceMod, int radius)
		{
			BlendState backupBlendState = graphicsDevice.BlendState;
			graphicsDevice.BlendState = BlendState.Opaque;

			this.ExecuteTechniqueDistortAndComputeDistance(shadowCasterMap, resultLight, shadowMapDistorted, "DistortAndComputeDistances", radius);

			Vector2 sizeVector = new Vector2(radius, radius);

			// Horizontal reduction
			this.ApplyHorizontalReduction(shadowMapDistorted, this.shadowMapDigested);

			this.distanceMod = distanceMod;

			switch (postEffect)
			{
				case PostEffect.LinearAttenuation:
					{
						this.ExecuteTechniqueDrawShadows(resultLight.PrintedLight, "DrawShadowsLinearAttenuation", this.shadowMapDigested, sizeVector);
					}
					break;

				case PostEffect.LinearAttenuation_BlurLow:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyLow", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyLowLinearAttenuation", sizeVector);
					}
					break;

				case PostEffect.LinearAttenuation_BlurMid:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyMid", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyMidLinearAttenuation", sizeVector);
					}
					break;

				case PostEffect.LinearAttenuation_BlurHigh:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyHigh", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyHighLinearAttenuation", sizeVector);
					}
					break;

				case PostEffect.CurveAttenuation:
					{
						this.ExecuteTechniqueDrawShadows(resultLight.PrintedLight, "DrawShadowsCurveAttenuation", this.shadowMapDigested, sizeVector);
					}
					break;

				case PostEffect.CurveAttenuation_BlurLow:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyLow", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyLowCurveAttenuation", sizeVector);
					}
					break;

				case PostEffect.CurveAttenuation_BlurMid:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyMid", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyMidCurveAttenuation", sizeVector);
					}
					break;

				case PostEffect.CurveAttenuation_BlurHigh:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyHigh", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyHighCurveAttenuation", sizeVector);
					}
					break;

				case PostEffect.Only_BlurLow:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyLow", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyLowNoAttenuation", sizeVector);
					}
					break;

				case PostEffect.Only_BlurMid:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyMid", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyMidNoAttenuation", sizeVector);
					}
					break;

				case PostEffect.Only_BlurHigh:
					{
						this.ExecuteTechniqueDrawShadows(shadowsRT, "DrawShadowsNoAttenuationPreBlur", this.shadowMapDigested, sizeVector);
						this.ExecuteTechniqueBlurH(shadowsRT, processedShadowsRT, "BlurHorizontallyHigh", sizeVector);
						this.ExecuteTechniqueBlurV(processedShadowsRT, resultLight.PrintedLight, "BlurVerticallyHighNoAttenuation", sizeVector);
					}
					break;

				default: //NOFX
					{
						this.ExecuteTechniqueDrawShadows(resultLight.PrintedLight, "DrawShadowsNoAttenuation", this.shadowMapDigested, sizeVector);
					}
					break;
			}
			graphicsDevice.BlendState = backupBlendState;
		}

		private void ExecuteTechniqueDistortAndComputeDistance(ShadowCasterMap shadowCasterMap, LightSource light, RenderTarget2D destination, string techniqueName, int radius)
		{
			graphicsDevice.SetRenderTarget(destination);
			graphicsDevice.Clear(Color.White);

			this.currenteRenderTargetSize.X = radius;
			this.currenteRenderTargetSize.Y = radius;
			this.lightsFX.ResolveShadowsEffect.Parameters["renderTargetSize"].SetValue(this.currenteRenderTargetSize);

			this.lightsFX.ResolveShadowsEffect.Parameters["lightRelativeZero"].SetValue(light.RelativeZeroHLSL(shadowCasterMap));

			Vector2 shadowCasterMapPortion = (light.Size * shadowCasterMap.PrecisionRatio) / shadowCasterMap.Size;
			this.lightsFX.ResolveShadowsEffect.Parameters["shadowCasterMapPortion"].SetValue(shadowCasterMapPortion);

			this.lightsFX.ResolveShadowsEffect.Parameters["InputTexture"].SetValue(shadowCasterMap.Map);

			this.lightsFX.ResolveShadowsEffect.CurrentTechnique = this.lightsFX.ResolveShadowsEffect.Techniques[techniqueName];

			this.lightsFX.ResolveShadowsEffect.CurrentTechnique.Passes[0].Apply();
			ShadowMapResolver.QuadRender.Render(this.graphicsDevice, Vector2.One * -1, Vector2.One);

			graphicsDevice.SetRenderTarget(null);
		}

		private void ExecuteTechniqueDrawShadows(RenderTarget2D destination, string techniqueName, Texture2D shadowMap, Vector2 renderTargetSize)
		{
			graphicsDevice.SetRenderTarget(destination);
			graphicsDevice.Clear(Color.White);

			this.lightsFX.ResolveShadowsEffect.Parameters["distanceMod"].SetValue(this.distanceMod);

			this.currenteRenderTargetSize.X = renderTargetSize.X;
			this.currenteRenderTargetSize.Y = renderTargetSize.Y;
			this.lightsFX.ResolveShadowsEffect.Parameters["renderTargetSize"].SetValue(this.currenteRenderTargetSize);

			this.lightsFX.ResolveShadowsEffect.Parameters["ShadowMapTexture"].SetValue(shadowMap);

			this.lightsFX.ResolveShadowsEffect.CurrentTechnique = this.lightsFX.ResolveShadowsEffect.Techniques[techniqueName];

			this.lightsFX.ResolveShadowsEffect.CurrentTechnique.Passes[0].Apply();
			QuadRender.Render(this.graphicsDevice, Vector2.One * -1, Vector2.One);

			graphicsDevice.SetRenderTarget(null);
		}

		private void ExecuteTechniqueBlurH(Texture2D source, RenderTarget2D destination, string techniqueName, Vector2 renderTargetSize)
		{
			graphicsDevice.SetRenderTarget(destination);
			graphicsDevice.Clear(Color.White);

			this.currenteRenderTargetSize.X = renderTargetSize.X;
			this.currenteRenderTargetSize.Y = renderTargetSize.Y;
			this.lightsFX.ResolveShadowsEffect.Parameters["renderTargetSize"].SetValue(this.currenteRenderTargetSize);

			this.lightsFX.ResolveShadowsEffect.Parameters["InputTexture"].SetValue(source);

			this.lightsFX.ResolveShadowsEffect.CurrentTechnique = this.lightsFX.ResolveShadowsEffect.Techniques[techniqueName];

			this.lightsFX.ResolveShadowsEffect.CurrentTechnique.Passes[0].Apply();
			ShadowMapResolver.QuadRender.Render(this.graphicsDevice, Vector2.One * -1, Vector2.One);

			graphicsDevice.SetRenderTarget(null);
		}

		private void ExecuteTechniqueBlurV(Texture2D source, RenderTarget2D destination, string techniqueName, Vector2 renderTargetSize)
		{
			graphicsDevice.SetRenderTarget(destination);
			graphicsDevice.Clear(Color.White);

			this.lightsFX.ResolveShadowsEffect.Parameters["distanceMod"].SetValue(this.distanceMod);

			this.currenteRenderTargetSize.X = renderTargetSize.X;
			this.currenteRenderTargetSize.Y = renderTargetSize.Y;
			this.lightsFX.ResolveShadowsEffect.Parameters["renderTargetSize"].SetValue(this.currenteRenderTargetSize);

			this.lightsFX.ResolveShadowsEffect.Parameters["InputTexture"].SetValue(source);

			this.lightsFX.ResolveShadowsEffect.CurrentTechnique = this.lightsFX.ResolveShadowsEffect.Techniques[techniqueName];

			this.lightsFX.ResolveShadowsEffect.CurrentTechnique.Passes[0].Apply();
			ShadowMapResolver.QuadRender.Render(this.graphicsDevice, Vector2.One * -1, Vector2.One);

			graphicsDevice.SetRenderTarget(null);
		}

		private void ApplyHorizontalReduction(RenderTarget2D source, RenderTarget2D destination)
		{
			RenderTarget2D s = source;
			RenderTarget2D d = this.reductionSteps[0].RenderTarget;

			this.lightsFX.ReductionEffect.CurrentTechnique = this.lightsFX.ReductionEffect.Techniques["HorizontalReduction"];

			for (int i = 0; i < this.reductionSteps.Length; i++)
			{
				if (i == this.reductionSteps.Length - 1)
					d = destination; // Here we go, it is the last reduction step and we save the shadow map in the destination rendertarget
				else
					d = this.reductionSteps[i].RenderTarget;

				graphicsDevice.SetRenderTarget(d);
				graphicsDevice.Clear(Color.White);

				this.lightsFX.ReductionEffect.Parameters["SourceTexture"].SetValue(s);
				Vector2 sourcePixelDimensions = new Vector2(1.0f / (float)s.Width, 1.0f / (float)s.Height);
				this.lightsFX.ReductionEffect.Parameters["SourcePixelDimensions"].SetValue(sourcePixelDimensions);
				this.lightsFX.ReductionEffect.Parameters["ReductionPower"].SetValue((byte)this.reductionSteps[i].ReductionPower);

				this.lightsFX.ReductionEffect.CurrentTechnique.Passes[0].Apply();
				ShadowMapResolver.QuadRender.Render(this.graphicsDevice, Vector2.One * -1, Vector2.One);

				graphicsDevice.SetRenderTarget(null);

				s = d;
			}
		}

		public void Dispose()
		{
			processedShadowsRT.Dispose();
			shadowMapDigested.Dispose();
			shadowMapDistorted.Dispose();
			shadowsRT.Dispose();
			distancesRT.Dispose();
		}
	}
}