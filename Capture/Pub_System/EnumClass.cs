using System;
using System.Collections.Generic;
using System.Text;

// 定义各种与图像有关的枚举变量
namespace ImageCapturing
{
    ///// <summary>
    ///// 定义几何图形对象
    ///// </summary> 
    //public enum drwType
    //{
    //    /// <summary>
    //    /// 圆点
    //    /// </summary>
    //    drwPoint,
    //    /// <summary>
    //    /// 圆十字点
    //    /// </summary>
    //    drwCrossCircle,              
    //    /// <summary>
    //    /// 融合点
    //    /// </summary>
    //    drwFusionPoint,              
    //    /// <summary>
    //    /// 十字点
    //    /// </summary>
    //    drwCross,                    
    //    /// <summary>
    //    /// 方十字点
    //    /// </summary>
    //    drwCrossSquare,              
    //    /// <summary>
    //    /// 数字十字点
    //    /// </summary>
    //    drwCrossNum,                 
    //    /// <summary>
    //    /// 水平分割线
    //    /// </summary>
    //    drwChopHorizontalLine,       
    //    /// <summary>
    //    /// 垂直分割线
    //    /// </summary>
    //    drwChopVerticalLine,       
    //    /// <summary>
    //    /// 斜线分割线
    //    /// </summary>
    //    drwChopInlinedLine,        
    //    /// <summary>
    //    /// 多边形
    //    /// </summary>
    //    drwPolygon,                
    //    /// <summary>
    //    /// 基本Beam
    //    /// </summary>
    //    drwBeam,                   
    //    /// <summary>
    //    /// 弧线Beam
    //    /// </summary>
    //    drwArcBeamEndLine,         
    //    /// <summary>
    //    /// 剂量
    //    /// </summary>
    //    drwDose,                   
    //    /// <summary>
    //    /// 剂量网格
    //    /// </summary>
    //    drwDoseMaske,              
    //    /// <summary>
    //    /// 十字标尺
    //    /// </summary>
    //    drwLineCross,              
    //    /// <summary>
    //    /// 字符串
    //    /// </summary>
    //    drwString,                 
    //    /// <summary>
    //    /// 内框,外框,并填充内部
    //    /// </summary>
    //    drwFillPath,               
    //    /// <summary>
    //    /// MLC
    //    /// </summary>
    //    drwMLC,                    
    //    /// <summary>
    //    /// 小人和片子位置
    //    /// </summary>
    //    drwPersonPositin,          
    //    /// <summary>
    //    /// 目标区域剂量及等级
    //    /// </summary>
    //    drwDoseLevelTarget,        
    //    /// <summary>
    //    /// MLC边界
    //    /// </summary>
    //    drwMLCWedge,               
    //    /// <summary>
    //    /// 图像网格
    //    /// </summary>
    //    drwImageGrid,              
    //    /// <summary>
    //    /// BEV上的网格
    //    /// </summary>
    //    drwBEVImageGrid,           
    //    /// <summary>
    //    /// 钨门
    //    /// </summary>
    //    drwJaw,		               
    //    /// <summary>
    //    /// POI点
    //    /// </summary>
    //    drwPOIPoint,               
    //    /// <summary>
    //    /// 权重点
    //    /// </summary>
    //    drwWeightPoint,            
    //    /// <summary>
    //    /// 归一点
    //    /// </summary>
    //    drwNormalPoint,            
    //    /// <summary>
    //    /// 增强点 
    //    /// </summary>
    //    drwEnhancePoint,           
    //    /// <summary>
    //    /// beam边界
    //    /// </summary>
    //    drwBeamEdge,               
    //    /// <summary>
    //    /// 勾画
    //    /// </summary>
    //    drwContour,                
    //    /// <summary>
    //    /// Bolus
    //    /// </summary>
    //    drwBolus,                  
    //    /// <summary>
    //    /// 画尺子
    //    /// </summary>
    //    drwRuler,                  
    //    /// <summary>
    //    /// 融合网格
    //    /// </summary>
    //    drwFusionGrid,             
    //    /// <summary>
    //    /// 弧线BEAM
    //    /// </summary>
    //    drwARCBeam,                
    //    /// <summary>
    //    /// 自动选野
    //    /// </summary>
    //    drwAutoAngle,              
    //    /// <summary>
    //    /// ISO中心角度
    //    /// </summary>
    //    drwAngleISO,               
    //    /// <summary>
    //    /// 框架点
    //    /// </summary>
    //    drawMarkPoint,             
    //    /// <summary>
    //    /// 相位信息
    //    /// </summary>
    //    drwPhase,                  
    //    /// <summary>
    //    /// 图像标尺
    //    /// </summary>
    //    drwImageRuler,
    //    /// <summary>
    //    /// 中心点
    //    /// </summary>
    //    drwIsocenter,
    //    /// <summary>
    //    /// 种子点识别范围
    //    /// </summary>
    //    /// jt add 2010.05.19
    //    drwSeedBound
    //}


    public enum ControlUnitType
    {
        /// <summary>
        /// 横截面
        /// </summary>
        Transverse,
        /// <summary>
        /// 冠状面
        /// </summary>
        Coronal,
        /// <summary>
        /// 失状面
        /// </summary>
        Sagittal,
        /// <summary>
        /// 斜切面
        /// </summary>
        Slope,
        /// <summary>
        /// 射野方向观
        /// </summary>
        BEV,
        /// <summary>
        /// 小网格控件（枚举图片网格控件）
        /// </summary>
        EnumerateImageGrid,
        /// <summary>
        /// 放大镜控件
        /// </summary>
        Magnifier,
        /// <summary>
        /// 人体三维显示控件
        /// </summary>
        Image3D,
        /// <summary>
        /// Default
        /// </summary>
        None
    }

    /// <summary>
    /// 定义图象切割方向
    /// </summary>
    public enum ImgType
    {
        /// <summary>
        /// 横切面
        /// </summary>
        imgTransverse,             
        /// <summary>
        /// 冠状面
        /// </summary>
        imgCoronal,                
        /// <summary>
        /// 失状面
        /// </summary>
        imgSagittal,               
        /// <summary>
        /// 斜切面
        /// </summary>
        imgOblique,                
        /// <summary>
        /// BEV图
        /// </summary>
        imgBEV,
        /// <summary>
        /// DRRxcjadd
        /// </summary>
        imgDRR,
        None
    }

    /// <summary>
    /// 定义填充类型
    /// </summary>
    public enum FillType
    {
        /// <summary>
        /// 线
        /// </summary>
        line,                      
        /// <summary>
        /// 面
        /// </summary>
        solid,                     
        /// <summary>
        /// ColorWash(等同于面)
        /// </summary>
        colorwash,                 
        /// <summary>
        /// 线，面都画
        /// </summary>
        line_solid,
        /// <summary>
        /// 虚线
        /// </summary>
        dotted
    }

    /// <summary>
    /// 定义绘图层
    /// </summary> 
    public enum LayerType
    {
        /// <summary>
        /// 背景层
        /// </summary>
        layerBackImage,            
        /// <summary>
        /// 网格层
        /// </summary>
        layerGrid,                 
        /// <summary>
        /// 勾画层
        /// </summary>
        layerContour,              
        /// <summary>
        /// 剂量层
        /// </summary>
        layerDose,                 
        /// <summary>
        /// Beam层
        /// </summary>
        layerBeam,                 
        /// <summary>
        /// 坐标线层
        /// </summary>
        layerAxis,                 
        /// <summary>
        /// 剂量网格层
        /// </summary>
        layerDoseGrid,             
        /// <summary>
        /// 光栅层
        /// </summary>
        layerMLC,                  
        /// <summary>
        /// POI层
        /// </summary>
        layerPOI,                  
        /// <summary>
        /// 融合标记点层
        /// </summary>
        layerFusionMarkers,        
        /// <summary>
        /// 射野边界层
        /// </summary>
        layerBeamEdge,             
        /// <summary>
        /// 权重点层
        /// </summary>
        layerWeightPoint,          
        /// <summary>
        /// Bolus层
        /// </summary>
        layerBolus,                
        /// <summary>
        /// Wedge层
        /// </summary>
        layerWedge,                
        /// <summary>
        /// 自动选野层
        /// </summary>
        layerAutoAngle,
        /// <summary>
        /// SeedBound
        /// </summary>
        layerSeedBound,
        /// <summary>
        /// 十字刻度尺层
        /// </summary>
        layerCrossRuler
    }

    /// <summary>
    /// 定义点的类型
    /// </summary>
    public enum SymbolType
    {
        //yongli 11/8/7 添加新的枚举字段
        /// <summary>
        /// 实心点
        /// </summary>
        SolidPoint,

        //yongli 11/8/7 添加新的枚举字段
        /// <summary>
        /// 空心点
        /// </summary>
        HollowPoint,

        //yongli 11/8/7 改Square为CrossSquare，取消GraphSquare类
        /// <summary> 
        /// 方形中有十字
        /// </summary>            
        CrossSquare,
        /// <summary>
        /// Rhombus-shaped
        /// </summary>
        Diamond,
        /// <summary> 
        /// Equilateral triangle
        /// </summary>
        Triangle,

        //yongli 11/8/7 将Circle改为CrossCircle（取消GraphCrossCircle类）
        /// <summary> 
        /// 圆中有十字
        /// e</summary>
        CrossCircle,
        /// <summary>
        /// "X" shaped
        /// </summary>
        XCross,
        /// <summary>
        /// "+" shaped 
        /// </summary>
        Plus,
        /// <summary>
        /// Asterisk-shaped 
        /// </summary>
        Star,
        /// <summary> 
        /// Unilateral triangle
        /// </summary>
        TriangleDown,

        //yongli 11/8/7 添加None枚举类型
        /// <summary>
        /// 点不显示（即不画点）
        /// </summary>
        None,
        Default
        ///// <summary> 
        ///// Square-shaped
        ///// </summary>            
        //Square,
        ///// <summary>
        ///// Rhombus-shaped
        ///// </summary>
        //Diamond,
        ///// <summary> 
        ///// Equilateral triangle
        ///// </summary>
        //Triangle,
        ///// <summary> 
        ///// Uniform circl
        ///// e</summary>
        //Circle,
        ///// <summary>
        ///// "X" shaped
        ///// </summary>
        //XCross,
        ///// <summary>
        ///// "+" shaped 
        ///// </summary>
        //Plus,
        ///// <summary>
        ///// Asterisk-shaped 
        ///// </summary>
        //Star,
        ///// <summary> 
        ///// Unilateral triangle
        ///// </summary>
        //TriangleDown,
        ///// <summary>
        ///// Horizontal dash 
        ///// </summary>           
        //Default            
    }


    /// <summary>
    /// 定义鼠标操作的类型
    /// </summary>
    public enum mouseMoveType
    {
        /// <summary>
        /// 无操作,感应
        /// </summary>
        moveNone,                  
        /// <summary>
        /// 拖动ＣＧｒａｐｈ对象
        /// </summary>
        moveDragObject,            
        /// <summary>
        /// 测量角度
        /// </summary>
        moveAngel,                 
        /// <summary>
        /// 测量长度
        /// </summary>
        moveRule,                  
        /// <summary>
        /// 调节窗宽窗高
        /// </summary>
        ajustWL,                   
        /// <summary>
        /// 放大镜
        /// </summary>
        moveMaginifer,             
        /// <summary>
        /// 放大
        /// </summary>
        moveZoomIn,                
        /// <summary>
        /// 缩小
        /// </summary>
        moveZoomOut,               
        /// <summary>
        /// 移动图像
        /// </summary>
        moveZoomMove,              
        /// <summary>
        /// 局部放大
        /// </summary>
        moveZoomRegion,            
        /// <summary>
        /// 修改斜切
        /// </summary>
        moveOblique,               
        /// <summary>
        /// 定义在PLan上的事件
        /// </summary>
        moveMoveOut,　　　　　　　
        /// <summary>
        /// 定义在PLan上的事件
        /// </summary>
        movePlan,                  
        /// <summary>
        /// Fusion移动副图像
        /// </summary>
        moveSlaveImage,            
        /// <summary>
        /// Fusion旋转副图像
        /// </summary>
        rotSlaveImage,             
        /// <summary>
        /// Fusion缩放副图像
        /// </summary>
        zoomSlaveImage,            
        /// <summary>
        /// ISO中心画角度
        /// </summary>
        moveAngleISO,
        /// <summary>
        /// 添加点
        /// </summary>
        /// 2008/11/25 add
        addFrameCoordPoint,
        /// <summary>
        /// 橡皮擦
        /// </summary>
        /// 2008/11/25 add
        Eraser
        
    }

    /// <summary>
    /// 缩放类型
    /// </summary>
    public enum ZoomType
    {
        /// <summary>
        /// 对当前的比例不进行改变
        /// </summary>
        ZoomNone,
        /// <summary>
        /// 原始比例
        /// </summary>
        ZoomNormal,
        /// <summary>
        /// 放大
        /// </summary>
        ZoomIn,
        /// <summary>
        /// 缩小
        /// </summary>
        ZoomOut,
        /// <summary>
        /// 局部放大
        /// </summary>
        ZoomRegion,
        /// <summary>
        /// 平移
        /// </summary>
        ZoomMove
    }

    public enum CursorType
    {
        Wheel,
        Eraser,
        Draw
    }

    public enum PersonPositionType
    {
        FaceUpAndHeadOut = 1,
        FaceUpAndHeadIn = 2,
        FaceOutAndHeadUp = 3,
        FaceLeftAndHeadUp = 4,
        FaceDownAndHeadOut = 5,
        FaceDownAndHeadIn = 6
    }

    public enum SmallPersonType
    {
        SupineF, //横截面  Supine head outside
        SupineH,  //横截面  Supine head inside
        Coronal,  //冠状面  
        Sagittal,  //矢状面
        NotationF, //横截面  Notation head outside
        NotationH,  //横截面  Notation head inside
    }
}
