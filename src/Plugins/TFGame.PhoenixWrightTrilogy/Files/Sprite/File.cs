using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using TF.Core.Files;

namespace TFGame.PhoenixWrightTrilogy.Files.Sprite
{
    public class File : DDSFile
    {
        private class Vector2F
        {
            public float X { get; set; }
            public float Y { get; set; }
        }

        private class Vector3F
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
        }

        private class Vector4F
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float W { get; set; }
        }

        private class RenderDataKeyPair
        {
            public uint[] First { get; set; }
            public long Second { get; set; }
        }

        private class Pointer
        {
            public int FileId { get; set; }
            public long PathId { get; set; }
        }

        private class AABB
        {
            public Vector3F Center { get; set; }
            public Vector3F Extent { get; set; }
        }

        private class SubMesh
        {
            public uint FirstByte { get; set; }
            public uint IndexCount { get; set; }
            public int Topology { get; set; }
            public uint BaseVertex { get; set; }
            public uint FirstVertex { get; set; }
            public uint VertexCount { get; set; }

            public AABB LocalAABB { get; set; }
        }

        private class ChannelInfo
        {
            public byte Stream { get; set; }
            public byte Offset { get; set; }
            public byte Format { get; set; }
            public byte Dimension { get; set; }
        }
        private class VertexData
        {
            public int CurrentChannels { get; set; }
            public uint VertexCount { get; set; }
            public ChannelInfo[] Channels { get; set; }

            public byte[] DataSize { get; set; }
        }
        private class SpriteRenderData
        {
            public Pointer Texture { get; set; }
            public Pointer AlphaTexture { get; set; }

            public SubMesh[] SubMeshes { get; set; }

            public byte[] IndexBuffer { get; set; }

            public VertexData Vertex { get; set; }
            public RectangleF TextureRect { get; set; }
            public Vector2F TextureRectOffset { get; set; }
            public Vector2F AtlasRectOffset { get; set; }

            public uint SettingsRaw { get; set; }

            public Vector4F UvTransform { get; set; }

            public float DownscaleMultiplier { get; set; }
        }
        private class Sprite
        {
            public RectangleF Rect { get; set; }
            public Vector2F Offset { get; set; }
            public Vector4F Border { get; set; }
            public float PixelsToUnits { get; set; }
            public Vector2F Pivot { get; set; }
            public int Extrude { get; set; }
            public bool IsPolygon { get; set; }
            public RenderDataKeyPair RenderDataKey { get; set; }

            public int[] AtlasTags { get; set; } // No se si es un array de enteros
            public Pointer SpriteAtlas { get; set; }

            public SpriteRenderData RenderData { get; set; }

            public Vector2F[][] PhysicsShape { get; set; }
        }
        public File(string path, string changesFolder) : base(path, changesFolder)
        {
        }

        protected override Tuple<Image, object> GetImage()
        {
            var ddsTuple = base.GetImage();


            return ddsTuple;
        }
    }
}
