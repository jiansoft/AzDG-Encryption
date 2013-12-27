import 'package:crypto/crypto.dart' show MD5, CryptoUtils;
import 'package:utf/utf.dart' ;

class AzDG {
  String _cipher = 'Private key';  
  AzDG({String cipher}) {   
    if (cipher != null)
    {
      _cipher = cipher;
    }
  }
  
  List<int> _cipherEncode(List<int> inputData)
  {
    var md5 = new MD5(); 
    md5.add(this._cipher.codeUnits);
    var cipherHash = CryptoUtils.bytesToHex(md5.close()).codeUnits; 
    var outData = new List<int>(inputData.length); 
    var loopCount = inputData.length;
    for (var i = 0; i < loopCount; i++) {
      outData[i] =  inputData[i] ^ cipherHash[i%32];
    }  
    return outData;
  }
  
  String Decrypt(String sourceText)
  {
    var decodeSourceText = _cipherEncode(CryptoUtils.base64StringToBytes(sourceText));
    var loopCount = decodeSourceText.length;
    var size = (loopCount / 2);
    var outData = new List<int>(size.toInt());
    for (int i = 0, j = 0; i < loopCount; ++i, ++j) {
      outData[j]  = (decodeSourceText[i] ^ decodeSourceText[++i]);
    }
    return decodeUtf8(outData);
  }
  
  String Crypt(String msg)
  {
    var now = new DateTime.now();
    var md5 = new MD5();   
    md5.add(now.toString().codeUnits);
    var noise = CryptoUtils.bytesToHex(md5.close()).codeUnits;
    var inputData = encodeUtf8(msg);
    var loopCount = inputData.length;
    var outData = new List<int>(loopCount*2);
    for (var i = 0, j = 0; i < loopCount ; i++,j++) {
        outData[j] = noise[i%32];
        j++;
        outData[j] = inputData[i] ^ noise[i%32];
    }
    return CryptoUtils.bytesToBase64(_cipherEncode(outData));
  }
}
