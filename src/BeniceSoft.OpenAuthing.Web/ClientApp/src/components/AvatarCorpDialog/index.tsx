import {
    Dialog,
    DialogContent,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog"
import { useCallback, useState } from "react"
import Cropper, { Area } from 'react-easy-crop'
import * as DialogPrimitive from "@radix-ui/react-dialog"
import { Button } from "../ui/button"
import { Slider } from "../ui/slider"
import { Minus, Plus } from "lucide-react"

function createImage(url: string): Promise<HTMLImageElement> {
    return new Promise((resolve, reject) => {
        const image = new Image()
        image.addEventListener('load', () => resolve(image))
        image.addEventListener('error', (error) => reject(error))
        image.setAttribute('crossOrigin', 'anonymous') // needed to avoid cross-origin issues on CodeSandbox
        image.src = url
    })
}

function getRadianAngle(degreeValue: number) {
    return (degreeValue * Math.PI) / 180
}

function rotateSize(width: number, height: number, rotation: number) {
    const rotRad = getRadianAngle(rotation)

    return {
        width:
            Math.abs(Math.cos(rotRad) * width) + Math.abs(Math.sin(rotRad) * height),
        height:
            Math.abs(Math.sin(rotRad) * width) + Math.abs(Math.cos(rotRad) * height),
    }
}

async function getCroppedImg(
    imageSrc: string,
    pixelCrop: Area,
    rotation = 0,
    flip = { horizontal: false, vertical: false }
): Promise<Blob | null> {
    const image = await createImage(imageSrc)
    const canvas = document.createElement('canvas')
    const ctx = canvas.getContext('2d')

    if (!ctx) {
        return null
    }

    const rotRad = getRadianAngle(rotation)

    // calculate bounding box of the rotated image
    const { width: bBoxWidth, height: bBoxHeight } = rotateSize(
        image.width,
        image.height,
        rotation
    )

    // set canvas size to match the bounding box
    canvas.width = bBoxWidth
    canvas.height = bBoxHeight

    // translate canvas context to a central location to allow rotating and flipping around the center
    ctx.translate(bBoxWidth / 2, bBoxHeight / 2)
    ctx.rotate(rotRad)
    ctx.scale(flip.horizontal ? -1 : 1, flip.vertical ? -1 : 1)
    ctx.translate(-image.width / 2, -image.height / 2)

    // draw rotated image
    ctx.drawImage(image, 0, 0)

    // croppedAreaPixels values are bounding box relative
    // extract the cropped image using these values
    const data = ctx.getImageData(
        pixelCrop.x,
        pixelCrop.y,
        pixelCrop.width,
        pixelCrop.height
    )

    // set canvas width to final desired crop size - this will clear existing context
    canvas.width = pixelCrop.width
    canvas.height = pixelCrop.height

    // paste generated rotate image at the top left corner
    ctx.putImageData(data, 0, 0)

    // As Base64 string
    // return canvas.toDataURL('image/jpeg');

    // As a blob
    return new Promise<Blob>((resolve, reject) => {
        canvas.toBlob((file) => {
            if (file === null) {
                reject('file is null')
                return
            }
            // resolve(URL.createObjectURL(file))
            resolve(file)
        }, 'image/jpeg')
    })
}


interface AvatarCorpDialogProps extends DialogPrimitive.DialogProps {
    imageSrc?: string
    onConfirm?: (croppedImage: Blob | null) => Promise<void>
}

const AvatarCorpDialog = ({
    open,
    onOpenChange,
    imageSrc,
    onConfirm
}: AvatarCorpDialogProps) => {
    const [scales, setScales] = useState<number[]>([1])
    const [crop, setCrop] = useState({ x: 0, y: 0 })
    const [croppedAreaPixels, setCroppedAreaPixels] = useState<Area | null>(null)
    const [processing, setProcessing] = useState<boolean>(false)

    const onCropComplete = useCallback((croppedArea: Area, croppedAreaPixels: Area) => {
        setCroppedAreaPixels(croppedAreaPixels)
    }, [])

    const handleConfirm = useCallback(async () => {
        setProcessing(true)
        try {
            if (onConfirm && imageSrc) {
                const croppedImage = await getCroppedImg(
                    imageSrc,
                    croppedAreaPixels!,
                    0
                )

                await onConfirm(croppedImage)
            }
        } catch (e) {
            console.error(e)
        } finally {
            setProcessing(false)
            onOpenChange && onOpenChange(false)
        }
    }, [croppedAreaPixels, imageSrc])

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent>
                <DialogHeader>
                    <DialogTitle>编辑图片</DialogTitle>
                </DialogHeader>
                <div className="w-full h-full min-h-[400px] relative flex flex-col gap-y-4 items-center justify-center pt-4 px-2">
                    <Cropper
                        classes={{ containerClassName: 'relative w-full h-full' }}
                        image={imageSrc}
                        crop={crop}
                        zoom={scales[0]}
                        aspect={1}
                        onCropChange={(location) => !processing && setCrop(location)}
                        onCropComplete={(a, b) => !processing && onCropComplete(a, b)}
                        onZoomChange={zoom => !processing && setScales([zoom])}
                    />
                    <div className="w-full flex gap-x-2 items-center justify-center">
                        <button disabled={processing}
                            onClick={() => setScales([Math.max(1, scales[0] - 0.1)])}>
                            <Minus className="w-4 h-4" />
                        </button>
                        <Slider className="w-48" value={scales}
                            disabled={processing}
                            max={3} min={1} step={.1}
                            onValueChange={setScales} />
                        <button disabled={processing}
                            onClick={() => setScales([Math.min(3, scales[0] + 0.1)])}>
                            <Plus className="w-4 h-4" />
                        </button>
                    </div>
                </div>
                <DialogFooter className="border-t -mx-6 px-6 pt-6">
                    <Button processing={processing}
                        onClick={() => onOpenChange && onOpenChange(false)} variant="outline">取消</Button>
                    <Button processing={processing}
                        onClick={handleConfirm}>确认</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    )
}

export default AvatarCorpDialog