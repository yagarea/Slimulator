using System.Drawing;
using System.Drawing.Imaging;
using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;

namespace Slimulator {
    public class AnimationBuffer {
        private readonly MediaOutput _buffer;
        public AnimationBuffer(string videoPath, int height, int width, int frameRate) {
            FFmpegLoader.FFmpegPath = "/usr/lib/";
            _buffer = MediaBuilder.CreateContainer(videoPath).WithVideo(new VideoEncoderSettings(
                width: width,
                height: height, 
                framerate: frameRate,
                codec: VideoCodec.H264)
            ).Create();
        }

        public void AddFrame(Bitmap frame) {
            Rectangle rect = new Rectangle(System.Drawing.Point.Empty, frame.Size);
            BitmapData bitLock = frame.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            ImageData bitmapImageData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, frame.Size);
            _buffer.Video.AddFrame(bitmapImageData);
            frame.UnlockBits(bitLock);
        }

        public void Export() {
            _buffer.Dispose();
        }
    }
}