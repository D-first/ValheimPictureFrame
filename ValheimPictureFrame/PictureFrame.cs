using System.Collections;
using UnityEngine;

namespace ValheimPictureFrame
{
    public class PictureFrame : MonoBehaviour
    {
        public float Interval { get; set; } = 5.0f;
        private string[] _textureNames = new string[0];
        private int _nextIndex = 0;

        public void StartAnimation(string[] textureNames)
        {
            _textureNames = textureNames;
            _nextIndex = 0;
            StartCoroutine(nameof(NextPicture));
        }

        public void StopAnimation()
        {
            StopCoroutine(nameof(NextPicture));
        }

        public void SetTexture(string fileName)
        {
            Renderer pictureRenderer = transform.Find("Pivot/New/Picture").gameObject.GetComponent<Renderer>();
            pictureRenderer.material.mainTexture = ValheimPictureFrame.textureCache.Load(fileName);
        }

        private IEnumerator NextPicture()
        {
            while (true)
            {

                if (_textureNames == null || _textureNames.Length == 0)
                {
                    yield break;
                }

                yield return new WaitForSeconds(Interval);

                _nextIndex = _nextIndex % _textureNames.Length;
                SetTexture(_textureNames[_nextIndex]);
                _nextIndex += 1;
            }
        }

    }
}
