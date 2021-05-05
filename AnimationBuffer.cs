using System.Drawing;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;

namespace Slimulator {
    public class AnimationBuffer {
        private MediaOutput buffer;
        private ImageConverter converter;

        public AnimationBuffer(string videoPath, int height, int width, int frameRate) {
            buffer = MediaBuilder.CreateContainer(videoPath).WithVideo(new VideoEncoderSettings(width: width,
                height: height, framerate: frameRate,
                codec: VideoCodec.H264)
            ).Create();
            converter = new ImageConverter();
        }

        public void AddFrame(Bitmap frame) {
            buffer.Video.AddFrame(ImageData.FromArray((byte[]) converter.ConvertTo(frame, typeof(byte[])),
                ImagePixelFormat.Argb32, frame.Width,frame.Height));
        }

        public void Export() {
            buffer.Dispose();
        }
    }
}