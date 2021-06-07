using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ValheimPictureFrame.Utils
{
    public class TextureCache
    {
        public string ImageBasePath { get; }
        private readonly Dictionary<string, Texture> cache = new Dictionary<string, Texture>();

        public TextureCache(string imageBasePath)
        {
            ImageBasePath = imageBasePath;
        }


        public Texture Load(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            string textureName = Path.ChangeExtension(name, Path.GetExtension(name).ToLower());

            if (cache.ContainsKey(textureName))
            {
                return cache[textureName];
            }


            if (!(textureName.EndsWith(".jpg") || textureName.EndsWith(".png")))
            {
                return null;
            }
            
            string path = Path.Combine(ImageBasePath, textureName);

            if (!File.Exists(path))
            {
                return null;
            }

            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);

            if (texture == null)
            {
                return null;
            }

            texture.name = textureName;
            cache.Add(textureName, texture);

            return texture;
        }

        public string[] LoadTextureNames(string dirPath)
        {
            var dir = Path.Combine(ImageBasePath, dirPath);
            string[] textureNames;
            try
            {
                textureNames = Directory.GetFiles(dir, "*.*", SearchOption.TopDirectoryOnly)
                         .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                         .ToArray();
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }

            return textureNames;
        }

        public bool IsDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var dir = Path.Combine(ImageBasePath, path);
            return Directory.Exists(dir);
        }
    }
}
