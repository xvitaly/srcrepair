/*
 * This file is a part of SRC Repair project. For more information
 * visit official site: https://www.easycoding.org/projects/srcrepair
 *
 * Copyright (c) 2011 - 2021 EasyCoding Team (ECTeam).
 * Copyright (c) 2005 - 2021 EasyCoding Team.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Text;

namespace srcrepair.gpg
{
    /// <summary>
    /// Class for working with GPG cryptographic functions.
    /// </summary>
    public static class GPGManager
    {
        /// <summary>
        /// Gets and returns GPG public key instance.
        /// </summary>
        /// <returns>Returns GPG public key instance.</returns>
        private static PgpPublicKey GetPublicKey()
        {
            PgpPublicKey Result = null;
            using (MemoryStream KeyStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.GPGPublicKey)))
            {
                using (Stream DecoderStream = PgpUtilities.GetDecoderStream(KeyStream))
                {
                    PgpPublicKeyRingBundle RingBundle = new PgpPublicKeyRingBundle(DecoderStream);
                    foreach (PgpPublicKeyRing Ring in RingBundle.GetKeyRings())
                    {
                        foreach (PgpPublicKey Key in Ring.GetPublicKeys())
                        {
                            if (Key.IsEncryptionKey && !Key.IsRevoked())
                            {
                                return Key;
                            }
                        }
                    }
                }
            }
            return Result;
        }

        /// <summary>
        /// Encrypts file with pre-defined GPG public key.
        /// </summary>
        /// <param name="FileName">Full path to the source file.</param>
        public static void EncryptFile(string FileName)
        {
            using (FileStream InputFileStream = new FileStream(FileName, FileMode.Open))
            {
                using (FileStream OutputFileStream = new FileStream(FileName + ".gpg", FileMode.Create))
                {
                    PgpEncryptedDataGenerator EncryptedDataGenerator = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Aes256, true, new SecureRandom());
                    EncryptedDataGenerator.AddMethod(GetPublicKey());
                    using (Stream EncryptedDataGeneratorStream = EncryptedDataGenerator.Open(OutputFileStream, new byte[4096]))
                    {
                        PgpCompressedDataGenerator CompressedDataGenerator = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
                        using (Stream CompressedDataGeneratorStream = CompressedDataGenerator.Open(EncryptedDataGeneratorStream))
                        {
                            PgpLiteralDataGenerator LiteralDataGenerator = new PgpLiteralDataGenerator();
                            using (Stream LiteralDataGeneratorStream = LiteralDataGenerator.Open(CompressedDataGeneratorStream, PgpLiteralData.Binary, String.Empty, InputFileStream.Length, DateTime.Now))
                            {
                                InputFileStream.CopyTo(LiteralDataGeneratorStream);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verifies the GPG-signature of the file.
        /// </summary>
        /// <param name="FileName">Full path to the detached signature file.</param>
        /// <returns>Returns True if signature check passed.</returns>
        public static bool VerifySignedFile(string FileName)
        {
            using (FileStream SignatureFileStream = new FileStream(FileName, FileMode.Open))
            {
                using (Stream SignatureFileDecoderStream = PgpUtilities.GetDecoderStream(SignatureFileStream))
                {
                    PgpObjectFactory pgpFact = new PgpObjectFactory(SignatureFileDecoderStream);
                    if (!(pgpFact.NextPgpObject() is PgpSignatureList sList)) throw new InvalidOperationException("Failed to create an instance of the PgpObjectFactory.");
                    PgpSignature firstSig = sList[0];
                    firstSig.InitVerify(GetPublicKey());
                    using (FileStream SourceFileStream = new FileStream(Path.Combine(Path.GetDirectoryName(FileName), Path.GetFileNameWithoutExtension(FileName)), FileMode.Open))
                    {
                        using (BufferedStream BufferedSourceFileStream = new BufferedStream(SourceFileStream))
                        {
                            const int BlockSize = 4096;
                            byte[] Buffer = new byte[BlockSize];
                            int BytesRead = 0;
                            while ((BytesRead = BufferedSourceFileStream.Read(Buffer, 0, BlockSize)) != 0)
                            {
                                firstSig.Update(Buffer, 0, BytesRead);
                            }
                        }
                    }
                    return firstSig.Verify();
                }
            }
        }
    }
}
