//
//  Unity_iOS_lib.m
//  Unity-iPhone
//
//  Created by 张琦凡 on 2018/8/21.
//

#import <Foundation/Foundation.h>
#import "Unity_iOS_lib.h"

@implementation LibClass

//在ios设备上打开某个窗口，根据sourceType的不同，可以是打开相机或者打开相册
-(void) OpenTarget:(UIImagePickerControllerSourceType) sourceType
{
    //这完全是ios开发中用objective-c写的打开相册的代码
    UIImagePickerController* picker = [[UIImagePickerController alloc] init];
    picker.delegate = self;
    picker.allowsEditing = true;
    picker.sourceType = sourceType;

    [self presentViewController:picker animated:YES completion:nil];
}

//这个方法好像是代理需要实现的契约方法，在确认选取图片完成后调用
-(void) imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary*)info
{
    [picker dismissViewControllerAnimated:YES completion:nil];
    UIImage* image = info[UIImagePickerControllerOriginalImage];
    
    NSString* imagePath = [self GetSavePath:@"SaveImage.png"];
    [self SaveImageFile:image toPath:imagePath];
}
//在取消选取图片时调用
-(void) imagePickerControllerDidCancel:(UIImagePickerController *)picker
{
    [picker dismissViewControllerAnimated:YES completion:nil];
    UnitySendMessage("Unity_iOS_Interaction", "CallbackFromUnity", "");
}

//定义代理方法中用到的两个方法
-(NSString*) GetSavePath:(NSString* )fileName
{
    NSArray* pathArr = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString* path = [pathArr objectAtIndex:0];
    return [path stringByAppendingPathComponent:fileName];
}

-(void) SaveImageFile:(UIImage*)image toPath:(NSString*)path
{
    //将图片按格式序列化为数据格式?
    NSData* data;
    if(UIImagePNGRepresentation(image))
    {
        data = UIImagePNGRepresentation(image);
    }else{
        data = UIImageJPEGRepresentation(image, 1);
    }
    
    [data writeToFile:path atomically:YES];
    
    //[IOS调用Unity]
    //ios代码中通过UnitySendMessage方法调用unity那边的方法
    //UnitySendMessage方法本身可以在xcode工程的Classes文件夹-Unity文件夹-UnityInterface.h中查看
    //参数1.unity中的脚本所在的物体名 2.方法名 3.string方法参数
    //在这里调用unity的方法来用图片做些什么
    UnitySendMessage("Unity_iOS_Interaction", "CallbackFromUnity", "imageName");
}

@end

#if defined(__cplusplus)
extern "C"
{
#endif
    
    //[Unity调用IOS]
    //这个方法在unity脚本中通过DllImport属性和extern IOS_OpenCamera()的声明来得到，这是从unity中调用ios代码的方式
    void IOS_OpenCamera()
    {
        LibClass* lib = [[LibClass alloc] init];
        UIViewController* viewController = UnityGetGLViewController();
        [viewController.view addSubview:lib.view];
        
        [lib OpenTarget:UIImagePickerControllerSourceTypeCamera];
    }
    
    void IOS_OpenAlbum()
    {
        LibClass* lib = [[LibClass alloc] init];
        UIViewController* viewController = UnityGetGLViewController();
        [viewController.view addSubview:lib.view];
        
        [lib OpenTarget:UIImagePickerControllerSourceTypePhotoLibrary];
    }
    
#if defined(__cplusplus)
}
#endif


