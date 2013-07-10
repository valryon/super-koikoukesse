require "openssl"
require "base64"

#
# Cryptographic stuff
#
class Encryption

  # We use the AES 256 bit cipher-block chaining symetric encryption
  @alg = "AES-256-CBC"

  # 32 bits key
  @rawkey = "p_o6u-e/t*+!".ljust(32, '_')

  def self.encrypt(clearText)

    cipher_enc = OpenSSL::Cipher::Cipher.new('AES-256-CBC')
    cipher_enc.encrypt
    cipher_enc.iv = @rawkey
    cipher_enc.key = @rawkey

    crypt = cipher_enc.update(clearText) + cipher_enc.final

    return Base64.encode64(crypt)
  end

  def self.decrypt(cipheredText)
    cipher = OpenSSL::Cipher::Cipher.new('AES-256-CBC')
    cipher.decrypt
    cipher.iv = @rawkey
    cipher.key = @rawkey

    clearText = cipher.update(Base64.decode64(cipheredText)) + cipher.final

    return clearText
  end

end
