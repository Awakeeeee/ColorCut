//
//  Unity_iOS_lib.h
//  Unity-iPhone
//
//  Created by 张琦凡 on 2018/8/21.
//

#ifndef Unity_iOS_lib_h
#define Unity_iOS_lib_h

//UnitySendMessage定义在这个库里，因此要在unity打包的工程里创建Unity_iOS_lib插件
#import <QuartzCore/CADisplayLink.h>

//继承UIViewController
@interface LibClass : UIViewController<UIImagePickerControllerDelegate, UINavigationControllerDelegate>
@end

#endif /* Unity_iOS_lib_h */
