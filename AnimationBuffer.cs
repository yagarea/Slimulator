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
            _buffer = MediaBuilder.CreateContainer(videoPath)
                                  .WithVideo(new VideoEncoderSettings(width, height, frameRate, VideoCodec.H264))
                                  .Create();
        }

        private static ImageData FrameToImageData(Bitmap bitmap) {
            Rectangle  rect            = new Rectangle(System.Drawing.Point.Empty, bitmap.Size);
            BitmapData bitLock         = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            ImageData  bitmapImageData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, bitmap.Size);
            bitmap.UnlockBits(bitLock);
            return bitmapImageData;
        }

        public void AddFrame(Bitmap frame) => _buffer.Video.AddFrame(FrameToImageData(frame));

        public void Export() => _buffer.Dispose();
    }
}