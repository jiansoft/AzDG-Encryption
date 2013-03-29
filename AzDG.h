//
//  AzDG.h
//
//  Created by Chen Eddie on 12/4/26.
//
#import <Foundation/Foundation.h>
@interface AzDG : NSObject
{   
}
-(id) initWithCipher : (NSString*) input;
-(NSString*) Encrypt:(NSString*) input;
-(NSString*) Decrypt:(NSString*) input;
@end

