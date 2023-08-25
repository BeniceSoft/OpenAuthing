import { useParams } from "umi"

export default () => {
    const { id } = useParams()

    return (
        <div>
            {id}
        </div>
    )
}