using System;
using System.Drawing;
using System.Drawing.Imaging;
using FFMediaToolkit;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;

namespace Slimulator {
    public class AnimationBuffer {
        private readonly MediaOutput buffer;
        private readonly ImageConverter converter;

        public AnimationBuffer(string videoPath, int height, int width, int frameRate) {
            FFmpegLoader.FFmpegPath = "/usr/lib/";
            buffer = MediaBuilder.CreateContainer(videoPath).WithVideo(new VideoEncoderSettings(width: width,
                height: height, framerate: frameRate,
                codec: VideoCodec.H264)
            ).Create();
            converter = new ImageConverter();
        }
        
        private static ImageData FrameToImageData(Bitmap bitmap) {
            var rect = new Rectangle(System.Drawing.Point.Empty, bitmap.Size);
            var bitLock = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            var bitmapData = ImageData.FromPointer(bitLock.Scan0, ImagePixelFormat.Bgr24, bitmap.Size);
            bitmap.UnlockBits(bitLock);
            return bitmapData;
        }

        public void AddFrame(Bitmap frame) {
            buffer.Video.AddFrame(FrameToImageData(frame));
        }

        public void Export() {
            buffer.Dispose();
        }
    }
}