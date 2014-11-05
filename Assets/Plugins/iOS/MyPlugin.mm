//
//  MyPlugin.m
//  MyPlugin
//
//  Created by Nelson_Mac on 2014/11/5.
//  Copyright (c) 2014年 Nelson_Mac. All rights reserved.
//

#import "MyPlugin.h"

@implementation MyPlugin

+(int)objcGetValue
{
    return 610;
}

@end

extern "C"
{
    int getValue()
    {
        return [MyPlugin objcGetValue];
    }
}


