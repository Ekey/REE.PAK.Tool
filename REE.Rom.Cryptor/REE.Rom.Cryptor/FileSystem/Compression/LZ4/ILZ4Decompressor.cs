using System;
namespace LZ4Sharp
{
    public interface ILZ4Decompressor
    {
        unsafe int Decompress(byte* compressedBuffer, byte* decompressedBuffer, int compressedSize, int maxDecompressedSize);
        byte[] Decompress(byte[] compressed);
        int Decompress(byte[] compressed, byte[] decompressedBuffer);
        int Decompress(byte[] compressedBuffer, byte[] decompressedBuffer, int compressedSize);
        int Decompress(byte[] compressedBuffer, int compressedPosition, byte[] decompressedBuffer, int decompressedPosition, int compressedSize);
        unsafe int DecompressKnownSize(byte* compressed, byte* decompressedBuffer, int decompressedSize);
        void DecompressKnownSize(byte[] compressed, byte[] decompressed);
        int DecompressKnownSize(byte[] compressed, byte[] decompressedBuffer, int decompressedSize);
    }
}
