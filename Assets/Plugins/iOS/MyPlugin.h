//
//  MyPlugin.h
//  MyPlugin
//
//  Created by Nelson_Mac on 2014/11/5.
//  Copyright (c) 2014年 Nelson_Mac. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MyPlugin : NSObject

+(int)objcGetValue;

@end

extern "C"
{
    int getValue();
}

