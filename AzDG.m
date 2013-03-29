
//
//base64 from QSUtilities https://github.com/mikeho/QSUtilities 
//MD5 from http://www.makebetterthings.com/iphone/how-to-get-md5-and-sha1-in-objective-c-ios-sdk/

#import "AzDG.h"
#import "MD5.h"
#import "QSStrings.h"

@interface AzDG ()
- (NSString *)getCipher;
- (NSData *)cipherCrypt:(NSData *)inputData;
@end

@implementation AzDG

NSString* cipher = @"Private key";

-(id) initWithCipher : (NSString*) input {
    self = [super init];    
    if (self != nil && input != nil && [input length] > 0) {        
        cipher = input;
    }        
    return self;
}

-(NSString*) getCipher {
    return cipher;
}

-(NSData*) cipherCrypt:(NSData*) inputData {    
    NSMutableData *outData = [[NSMutableData alloc]init];
    int loopCount = [inputData length];  
    const unsigned char *inputChar = [inputData bytes];    
    const unsigned char *cipherHash = [[[MD5 encode: [self getCipher]] dataUsingEncoding:NSASCIIStringEncoding] bytes];   
    int i = 0;
    while (i < loopCount) {         
        UInt8 b = inputChar[i] ^ cipherHash[i % 32];
        [outData appendBytes:&b length:sizeof(UInt8)];  
        i++;
    }   
    return outData;  
}


-(NSString*) Encrypt:(NSString*) input {  
    NSData *inputData = [input dataUsingEncoding:NSUTF8StringEncoding];
    NSMutableData *outData = [[NSMutableData alloc]init];    
    int loopCount = [inputData length];  
    const UInt8 *inputChar = [inputData bytes];
    const UInt8 *noiseChar = [[[MD5 encode:[NSString stringWithFormat:@"%@",[NSDate date]]]
                                       dataUsingEncoding:NSASCIIStringEncoding] bytes];
  
    int i = 0;
    while (i < loopCount) {      
        [outData appendBytes:&noiseChar[i%32] length:sizeof(UInt8)];
        UInt8 b = inputChar[i] ^ noiseChar[i % 32];
        [outData appendBytes:&b length:sizeof(UInt8)];
        i++;
    }
    return [QSStrings encodeBase64WithData:[self cipherCrypt:outData]];
}

-(NSString*) Decrypt:(NSString*) input {
    NSData* inputData = [self cipherCrypt:[QSStrings decodeBase64WithString:input]];
    NSMutableData *outData = [[NSMutableData alloc] init];
    int loopCount = [inputData length]; 
    const UInt8 *inputChar = [inputData bytes];    
    int i = 0;
    while (i < loopCount) {
        UInt8 b = inputChar[i] ^ inputChar[i + 1];
        [outData appendBytes:&b length:sizeof(UInt8)];
        i = i + 2;
    }
    return [[NSString alloc] initWithData:outData encoding:NSUTF8StringEncoding]; 
}
@end

