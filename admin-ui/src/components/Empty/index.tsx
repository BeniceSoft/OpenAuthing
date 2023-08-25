import emptyIcon from '@/assets/icons/empty.png';

export default ({
    isEmpty,
    image,
    description,
    children
}: { isEmpty: boolean, image?: React.ReactNode, description?: React.ReactNode, children?: JSX.Element }) => {
    return (
        isEmpty ?
            <div className="w-full">
                <div className='my-16'>
                    {image ?? <img src={emptyIcon} className="mx-auto w-48" />}
                    {description &&
                        <p className="text-center text-gray-400 text-sm mt-4">
                            {description}
                        </p>
                    }
                </div>
            </div> :
            <>{children}</>
    )
}