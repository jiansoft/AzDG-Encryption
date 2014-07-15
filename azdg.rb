#!/usr/bin/env ruby
# encoding: UTF-8
require 'digest'
require 'base64'

class AzDG
    attr_accessor :cipher
    def initialize(cipher = "2407Tw0kooco0wT7042")
        @cipherHash = Digest::MD5.hexdigest(cipher)
    end

    def cipherEncode(inputData)   
        outData = Array.new(inputData.length)
        for i in (0..inputData.length-1) 
            outData[i] = (inputData[i] ^ @cipherHash[i%32].ord)
        end    
        return outData
    end

    def Encrypt(inputData)  
        noise = Digest::MD5.hexdigest(Time.new.strftime("%Y-%m-%d %H:%M:%S"))
        aryInputData = inputData.bytes.to_a        
        outData = Array.new(aryInputData.length * 2)
        for i in (0..aryInputData.length-1) 
            outData[(i*2)] = noise[i%32].ord     
            outData[(i*2)+1] = aryInputData[i] ^ noise[i%32].ord 
        end        
        return Base64.encode64(cipherEncode(outData).pack('c*'))
    end

    def Decrypt(inputData) 
        decodeSourceText = cipherEncode(Base64.decode64(inputData).bytes.to_a)
        outData = Array.new()
        i = 0           
        while i < decodeSourceText.length do 
            outData.push(decodeSourceText[i].ord ^ decodeSourceText[i+1].ord)    
            i += 2
        end
        return outData.pack('c*').force_encoding('UTF-8')
    end
end     