using System;
using System.Drawing;
using System.Drawing.Imaging;
using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;

namespace Slimulator {
    public class AnimationBuffer {
        private readonly MediaOutput buffer;
        public AnimationBuffer(string videoPath, int height, int width, int frameRate) {
            FFmpegLoader.FFmpegPath = "/usr/lib/";
            buffer = MediaBuilder.CreateContainer(videoPath).WithVideo(new VideoEncoderSettings(width: width,
                height: height, framerate: frameRate,
                codec: VideoCodec.H264)
            ).Create();
        }

        private static ImageData FrameToImageData(Bitmap bitmap) {
            Rectangle rect = new Rectangle(System.Drawing.Point.Empty, bitmap.Size);
            BitmapData bitLock = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            ImageData bitmapImageData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, bitmap.Size);
            bitmap.UnlockBits(bitLock);
            return bitmapImageData;
        }

        public void AddFrame(Bitmap frame) {
            buffer.Video.AddFrame(FrameToImageData(frame));
        }

        public void Export() {
            buffer.Dispose();
        }
    }
}