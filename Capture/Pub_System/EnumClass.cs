using System;
using System.Collections.Generic;
using System.Text;

// ���������ͼ���йص�ö�ٱ���
namespace ImageCapturing
{
    ///// <summary>
    ///// ���弸��ͼ�ζ���
    ///// </summary> 
    //public enum drwType
    //{
    //    /// <summary>
    //    /// Բ��
    //    /// </summary>
    //    drwPoint,
    //    /// <summary>
    //    /// Բʮ�ֵ�
    //    /// </summary>
    //    drwCrossCircle,              
    //    /// <summary>
    //    /// �ںϵ�
    //    /// </summary>
    //    drwFusionPoint,              
    //    /// <summary>
    //    /// ʮ�ֵ�
    //    /// </summary>
    //    drwCross,                    
    //    /// <summary>
    //    /// ��ʮ�ֵ�
    //    /// </summary>
    //    drwCrossSquare,              
    //    /// <summary>
    //    /// ����ʮ�ֵ�
    //    /// </summary>
    //    drwCrossNum,                 
    //    /// <summary>
    //    /// ˮƽ�ָ���
    //    /// </summary>
    //    drwChopHorizontalLine,       
    //    /// <summary>
    //    /// ��ֱ�ָ���
    //    /// </summary>
    //    drwChopVerticalLine,       
    //    /// <summary>
    //    /// б�߷ָ���
    //    /// </summary>
    //    drwChopInlinedLine,        
    //    /// <summary>
    //    /// �����
    //    /// </summary>
    //    drwPolygon,                
    //    /// <summary>
    //    /// ����Beam
    //    /// </summary>
    //    drwBeam,                   
    //    /// <summary>
    //    /// ����Beam
    //    /// </summary>
    //    drwArcBeamEndLine,         
    //    /// <summary>
    //    /// ����
    //    /// </summary>
    //    drwDose,                   
    //    /// <summary>
    //    /// ��������
    //    /// </summary>
    //    drwDoseMaske,              
    //    /// <summary>
    //    /// ʮ�ֱ��
    //    /// </summary>
    //    drwLineCross,              
    //    /// <summary>
    //    /// �ַ���
    //    /// </summary>
    //    drwString,                 
    //    /// <summary>
    //    /// �ڿ�,���,������ڲ�
    //    /// </summary>
    //    drwFillPath,               
    //    /// <summary>
    //    /// MLC
    //    /// </summary>
    //    drwMLC,                    
    //    /// <summary>
    //    /// С�˺�Ƭ��λ��
    //    /// </summary>
    //    drwPersonPositin,          
    //    /// <summary>
    //    /// Ŀ������������ȼ�
    //    /// </summary>
    //    drwDoseLevelTarget,        
    //    /// <summary>
    //    /// MLC�߽�
    //    /// </summary>
    //    drwMLCWedge,               
    //    /// <summary>
    //    /// ͼ������
    //    /// </summary>
    //    drwImageGrid,              
    //    /// <summary>
    //    /// BEV�ϵ�����
    //    /// </summary>
    //    drwBEVImageGrid,           
    //    /// <summary>
    //    /// ����
    //    /// </summary>
    //    drwJaw,		               
    //    /// <summary>
    //    /// POI��
    //    /// </summary>
    //    drwPOIPoint,               
    //    /// <summary>
    //    /// Ȩ�ص�
    //    /// </summary>
    //    drwWeightPoint,            
    //    /// <summary>
    //    /// ��һ��
    //    /// </summary>
    //    drwNormalPoint,            
    //    /// <summary>
    //    /// ��ǿ�� 
    //    /// </summary>
    //    drwEnhancePoint,           
    //    /// <summary>
    //    /// beam�߽�
    //    /// </summary>
    //    drwBeamEdge,               
    //    /// <summary>
    //    /// ����
    //    /// </summary>
    //    drwContour,                
    //    /// <summary>
    //    /// Bolus
    //    /// </summary>
    //    drwBolus,                  
    //    /// <summary>
    //    /// ������
    //    /// </summary>
    //    drwRuler,                  
    //    /// <summary>
    //    /// �ں�����
    //    /// </summary>
    //    drwFusionGrid,             
    //    /// <summary>
    //    /// ����BEAM
    //    /// </summary>
    //    drwARCBeam,                
    //    /// <summary>
    //    /// �Զ�ѡҰ
    //    /// </summary>
    //    drwAutoAngle,              
    //    /// <summary>
    //    /// ISO���ĽǶ�
    //    /// </summary>
    //    drwAngleISO,               
    //    /// <summary>
    //    /// ��ܵ�
    //    /// </summary>
    //    drawMarkPoint,             
    //    /// <summary>
    //    /// ��λ��Ϣ
    //    /// </summary>
    //    drwPhase,                  
    //    /// <summary>
    //    /// ͼ����
    //    /// </summary>
    //    drwImageRuler,
    //    /// <summary>
    //    /// ���ĵ�
    //    /// </summary>
    //    drwIsocenter,
    //    /// <summary>
    //    /// ���ӵ�ʶ��Χ
    //    /// </summary>
    //    /// jt add 2010.05.19
    //    drwSeedBound
    //}


    public enum ControlUnitType
    {
        /// <summary>
        /// �����
        /// </summary>
        Transverse,
        /// <summary>
        /// ��״��
        /// </summary>
        Coronal,
        /// <summary>
        /// ʧ״��
        /// </summary>
        Sagittal,
        /// <summary>
        /// б����
        /// </summary>
        Slope,
        /// <summary>
        /// ��Ұ�����
        /// </summary>
        BEV,
        /// <summary>
        /// С����ؼ���ö��ͼƬ����ؼ���
        /// </summary>
        EnumerateImageGrid,
        /// <summary>
        /// �Ŵ󾵿ؼ�
        /// </summary>
        Magnifier,
        /// <summary>
        /// ������ά��ʾ�ؼ�
        /// </summary>
        Image3D,
        /// <summary>
        /// Default
        /// </summary>
        None
    }

    /// <summary>
    /// ����ͼ���и��
    /// </summary>
    public enum ImgType
    {
        /// <summary>
        /// ������
        /// </summary>
        imgTransverse,             
        /// <summary>
        /// ��״��
        /// </summary>
        imgCoronal,                
        /// <summary>
        /// ʧ״��
        /// </summary>
        imgSagittal,               
        /// <summary>
        /// б����
        /// </summary>
        imgOblique,                
        /// <summary>
        /// BEVͼ
        /// </summary>
        imgBEV,
        /// <summary>
        /// DRRxcjadd
        /// </summary>
        imgDRR,
        None
    }

    /// <summary>
    /// �����������
    /// </summary>
    public enum FillType
    {
        /// <summary>
        /// ��
        /// </summary>
        line,                      
        /// <summary>
        /// ��
        /// </summary>
        solid,                     
        /// <summary>
        /// ColorWash(��ͬ����)
        /// </summary>
        colorwash,                 
        /// <summary>
        /// �ߣ��涼��
        /// </summary>
        line_solid,
        /// <summary>
        /// ����
        /// </summary>
        dotted
    }

    /// <summary>
    /// �����ͼ��
    /// </summary> 
    public enum LayerType
    {
        /// <summary>
        /// ������
        /// </summary>
        layerBackImage,            
        /// <summary>
        /// �����
        /// </summary>
        layerGrid,                 
        /// <summary>
        /// ������
        /// </summary>
        layerContour,              
        /// <summary>
        /// ������
        /// </summary>
        layerDose,                 
        /// <summary>
        /// Beam��
        /// </summary>
        layerBeam,                 
        /// <summary>
        /// �����߲�
        /// </summary>
        layerAxis,                 
        /// <summary>
        /// ���������
        /// </summary>
        layerDoseGrid,             
        /// <summary>
        /// ��դ��
        /// </summary>
        layerMLC,                  
        /// <summary>
        /// POI��
        /// </summary>
        layerPOI,                  
        /// <summary>
        /// �ںϱ�ǵ��
        /// </summary>
        layerFusionMarkers,        
        /// <summary>
        /// ��Ұ�߽��
        /// </summary>
        layerBeamEdge,             
        /// <summary>
        /// Ȩ�ص��
        /// </summary>
        layerWeightPoint,          
        /// <summary>
        /// Bolus��
        /// </summary>
        layerBolus,                
        /// <summary>
        /// Wedge��
        /// </summary>
        layerWedge,                
        /// <summary>
        /// �Զ�ѡҰ��
        /// </summary>
        layerAutoAngle,
        /// <summary>
        /// SeedBound
        /// </summary>
        layerSeedBound,
        /// <summary>
        /// ʮ�̶ֿȳ߲�
        /// </summary>
        layerCrossRuler
    }

    /// <summary>
    /// ����������
    /// </summary>
    public enum SymbolType
    {
        //yongli 11/8/7 ����µ�ö���ֶ�
        /// <summary>
        /// ʵ�ĵ�
        /// </summary>
        SolidPoint,

        //yongli 11/8/7 ����µ�ö���ֶ�
        /// <summary>
        /// ���ĵ�
        /// </summary>
        HollowPoint,

        //yongli 11/8/7 ��SquareΪCrossSquare��ȡ��GraphSquare��
        /// <summary> 
        /// ��������ʮ��
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

        //yongli 11/8/7 ��Circle��ΪCrossCircle��ȡ��GraphCrossCircle�ࣩ
        /// <summary> 
        /// Բ����ʮ��
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

        //yongli 11/8/7 ���Noneö������
        /// <summary>
        /// �㲻��ʾ���������㣩
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
    /// ����������������
    /// </summary>
    public enum mouseMoveType
    {
        /// <summary>
        /// �޲���,��Ӧ
        /// </summary>
        moveNone,                  
        /// <summary>
        /// �϶��ãǣ�������
        /// </summary>
        moveDragObject,            
        /// <summary>
        /// �����Ƕ�
        /// </summary>
        moveAngel,                 
        /// <summary>
        /// ��������
        /// </summary>
        moveRule,                  
        /// <summary>
        /// ���ڴ�����
        /// </summary>
        ajustWL,                   
        /// <summary>
        /// �Ŵ�
        /// </summary>
        moveMaginifer,             
        /// <summary>
        /// �Ŵ�
        /// </summary>
        moveZoomIn,                
        /// <summary>
        /// ��С
        /// </summary>
        moveZoomOut,               
        /// <summary>
        /// �ƶ�ͼ��
        /// </summary>
        moveZoomMove,              
        /// <summary>
        /// �ֲ��Ŵ�
        /// </summary>
        moveZoomRegion,            
        /// <summary>
        /// �޸�б��
        /// </summary>
        moveOblique,               
        /// <summary>
        /// ������PLan�ϵ��¼�
        /// </summary>
        moveMoveOut,��������������
        /// <summary>
        /// ������PLan�ϵ��¼�
        /// </summary>
        movePlan,                  
        /// <summary>
        /// Fusion�ƶ���ͼ��
        /// </summary>
        moveSlaveImage,            
        /// <summary>
        /// Fusion��ת��ͼ��
        /// </summary>
        rotSlaveImage,             
        /// <summary>
        /// Fusion���Ÿ�ͼ��
        /// </summary>
        zoomSlaveImage,            
        /// <summary>
        /// ISO���Ļ��Ƕ�
        /// </summary>
        moveAngleISO,
        /// <summary>
        /// ��ӵ�
        /// </summary>
        /// 2008/11/25 add
        addFrameCoordPoint,
        /// <summary>
        /// ��Ƥ��
        /// </summary>
        /// 2008/11/25 add
        Eraser
        
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum ZoomType
    {
        /// <summary>
        /// �Ե�ǰ�ı��������иı�
        /// </summary>
        ZoomNone,
        /// <summary>
        /// ԭʼ����
        /// </summary>
        ZoomNormal,
        /// <summary>
        /// �Ŵ�
        /// </summary>
        ZoomIn,
        /// <summary>
        /// ��С
        /// </summary>
        ZoomOut,
        /// <summary>
        /// �ֲ��Ŵ�
        /// </summary>
        ZoomRegion,
        /// <summary>
        /// ƽ��
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
        SupineF, //�����  Supine head outside
        SupineH,  //�����  Supine head inside
        Coronal,  //��״��  
        Sagittal,  //ʸ״��
        NotationF, //�����  Notation head outside
        NotationH,  //�����  Notation head inside
    }
}
