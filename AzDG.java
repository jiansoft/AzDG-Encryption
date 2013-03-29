import java.io.UnsupportedEncodingException;
import java.util.*;
import java.security.*; 
	
public class AzDg { 
	private static String strPrivateKey = "Private key";
	
	public static byte[] encode(byte[] argAbteSource, byte[] argAbteEncryptKey) {
		argAbteEncryptKey = new Md5().getMD5ofStr(new String(argAbteEncryptKey)).toLowerCase().getBytes();
		byte bteCRCLength = 0;  
		byte[] abteReturn = new byte[argAbteSource.length];
		for (int i = 0; i < argAbteSource.length; ++i) {
			bteCRCLength = (bteCRCLength > 31) ? 0 : bteCRCLength;
			abteReturn[i] = (byte)(argAbteSource[i] ^ argAbteEncryptKey[bteCRCLength++]);
		}
		return abteReturn;
	}
	
	public static String encrypt(String argStrSource) {
  		return encrypt(argStrSource, strPrivateKey);
 	}
	
 	public static String encrypt(String argStrSource, String argStrPrivateKey) {
  		return encrypt(argStrSource, argStrPrivateKey,"UTF8");
 	}

 	public static String encrypt(String argStrSource, String argStrPrivateKey, String argStrCharset) {
  		String strReturn = null;
  		try	{
   			strReturn = new String(encrypt(argStrSource.getBytes(argStrCharset), argStrPrivateKey.getBytes(argStrCharset)),argStrCharset);
  		}
  		catch (UnsupportedEncodingException e) {
   			e.printStackTrace();
  		}
 		return strReturn;
 	}

 	public static byte[] encrypt(byte[] argStrSource, byte[] argBteKey) {  		
  		byte[] abteEncryptKey = new Md5().getMD5ofStr(Long.toString( Calendar.getInstance().getTimeInMillis())).getBytes();
  		byte bteCRCLength = 0;  
  		byte[] abteReturn = new byte[argStrSource.length * 2];  
		for (int i = 0, j = 0; i < argStrSource.length; ++i, ++j) {
			bteCRCLength = bteCRCLength > 31 ? 0 : bteCRCLength;  
			abteReturn[j] = abteEncryptKey[bteCRCLength];
			++j;
			abteReturn[j] = (byte)(argStrSource[i] ^ abteEncryptKey[bteCRCLength++]);
		}		
  		return Base64.encode(encode(abteReturn, argBteKey));
	}
 	
 	public static String decrypt(String argStrSource) {
 		return decrypt(argStrSource,strPrivateKey);
	}
 	
	public static String decrypt(String argStrSource, String argStrPrivateKey)	{
		return decrypt(argStrSource, argStrPrivateKey, "UTF8");
	}
	
	public static String decrypt(String argStrSource, String argStrPrivateKey, String argStrCharset) {
		String strReturn ="";
		try {
			strReturn= new String(decrypt(Base64.decode(argStrSource), argStrPrivateKey.getBytes(argStrCharset)),argStrCharset);
		}
		catch (UnsupportedEncodingException e) {
   			e.printStackTrace();
  		}
 		return strReturn;
	}

	public static byte[] decrypt(byte[] argStrSource, byte[] argStrPrivateKey) {
		argStrSource = encode(argStrSource, argStrPrivateKey);
		byte[] abteReturn = new byte[(int)(argStrSource.length / 2)]; 
		for (int i = 0, j = 0; i < argStrSource.length; ++i, ++j) {
			abteReturn[j] = (byte)(argStrSource[i] ^ argStrSource[++i] );
		}  
		return abteReturn;
	}
		
	public static byte[] md5Byte(String d) throws NoSuchAlgorithmException {
		MessageDigest md = MessageDigest.getInstance("MD5"); 		
		md.update(d.getBytes()); 
		return  md.digest(); 		
	}		
}