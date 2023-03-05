﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomRenderPipeline
{
    [CreateAssetMenu(menuName = "Rendering/ " + nameof(SpaceRunPipelineRenderAsset))]
    public class SpaceRunPipelineRenderAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new SpaceRunPipelineRender();
        }
    }
}
