using UnityEngine;
using UnityEngine.UI;

public class TextGradation : BaseMeshEffect
{
    public Color32 Color1 = Color.red;
    public Color32 Color2 = Color.white;

    public override void ModifyMesh(VertexHelper vertexHelper)
    {
        if (!IsActive())
        {
            return;
        }

        UIVertex uiVertex = new UIVertex();
        int index = 0;

        for (int i = 0; i < vertexHelper.currentVertCount; i++)
        {
            vertexHelper.PopulateUIVertex(ref uiVertex, i);

            //横グラデーション
            if (index == 0 || index == 3)
            {
                uiVertex.color = Color1;
            }
            else
            {
                uiVertex.color = Color2;
            }

            vertexHelper.SetUIVertex(uiVertex, i);

            index++;
            if (index > 3)
            {
                index = 0;
            }
        }
    }
}