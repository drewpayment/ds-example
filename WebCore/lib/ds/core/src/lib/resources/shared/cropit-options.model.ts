// default options
// http://scottcheng.github.io/cropit/
// $preview: $imageCropper.find('.cropit-preview')
// $fileInput: $imageCropper.find('input.cropit-image-input')
// $zoomSlider: $imageCropper.find('input.cropit-image-zoom-input')
// width: null
// height: null
// imageBackground: false
// imageBackgroundBorderWidth: [0, 0, 0, 0]
// exportZoom: 1
// allowDragNDrop: true
// minZoom: 'fill'
// maxZoom: 1
// initialZoom: 'min'
// freeMove: false
// smallImage: 'reject'
// onFileChange: (evt) => {} called when user inputs an image
// onFileReaderError: () => {}
// onFileReaderError: () => {}
// onImageLoading: () => {}
// onImageError: (err, err.code, err.message) => {}
// onZoomEnabled: () => {}
// onZoomDisabled: () => {}
// onZoomChange: () => {}
// onOffsetChange: () => {}
export interface ICropitOptions {
    $preview?:ng.IAugmentedJQuery,
    $fileInput?:ng.IAugmentedJQuery,
    $zoomSlider?:ng.IAugmentedJQuery,
    width?:number | null,
    height?:number | null,
    imageBackground?:boolean,
    imageBackgroundBorderWith?:number,
    exportZoom?:number,
    allowDragNDrop?:boolean,
    minZoom?:number | string,
    maxZoom?:number | string,
    initialZoom?:string,
    freeMove?:boolean,
    smallImage?:string,
    onFileChange?:(evt:any) => void,
    onFileReaderError?:() => void,
    onImageLoading?:() => void,
    onImageError?:(err:{}, code:any, message:string) => void,
    onZoomEnabled?:() => void,
    onZoomDisabled?:() => void,
    onZoomChanged?:() => void,
    onOffsetChange?:() => void
}