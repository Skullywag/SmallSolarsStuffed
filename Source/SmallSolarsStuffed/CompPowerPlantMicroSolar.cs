using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace SmallSolarsStuffed
{
    [StaticConstructorOnStartup]
    public class CompPowerPlantMicroSolar : CompPowerPlant
    {
        private float FullSunPower;

        private const float NightPower = 0f;

        private static readonly Vector2 BarSize = new Vector2(1.725f, 0.14f);

        private static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f));

        private static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f));

        protected override float DesiredPowerOutput
        {
            get
            {
                if (this.parent.def.MadeFromStuff)
                {
                    switch (this.parent.Stuff.defName)
                    {
                        case "Silver":
                            FullSunPower = 1400f;
                            break;
                        case "Gold":
                            FullSunPower = 1500f;
                            break;
                        case "Steel":
                            FullSunPower = 1300f;
                            break;
                        case "Plasteel":
                            FullSunPower = 1700f;
                            break;
                        case "Uranium":
                            FullSunPower = 1800f;
                            break;
                        default:
                            FullSunPower = 1300f;
                            break;
                    }
                }
                return Mathf.Lerp(0f, FullSunPower, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
            }
        }

        private float RoofedPowerOutputFactor
        {
            get
            {
                int num = 0;
                int num2 = 0;
                foreach (IntVec3 current in this.parent.OccupiedRect())
                {
                    num++;
                    if (this.parent.Map.roofGrid.Roofed(current))
                    {
                        num2++;
                    }
                }
                return (float)(num - num2) / (float)num;
            }
        }

        public override void PostDraw()
        {
            base.PostDraw();
            GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
            r.center = this.parent.DrawPos + Vector3.up * 0.1f;
            r.size = CompPowerPlantMicroSolar.BarSize;
            r.fillPercent = base.PowerOutput / FullSunPower;
            r.filledMat = CompPowerPlantMicroSolar.PowerPlantSolarBarFilledMat;
            r.unfilledMat = CompPowerPlantMicroSolar.PowerPlantSolarBarUnfilledMat;
            r.margin = 0.15f;
            Rot4 rotation = this.parent.Rotation;
            rotation.Rotate(RotationDirection.Clockwise);
            r.rotation = rotation;
            GenDraw.DrawFillableBar(r);
        }
    }
}
