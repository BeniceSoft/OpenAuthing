import { SearchIcon } from "lucide-react"
import { ChangeEventHandler } from "react"

type SearchInputProps = {
    placeholder?: string
    value?: string
    onChange?: ChangeEventHandler<HTMLInputElement>
}

const SearchInput: React.FC<SearchInputProps> = ({ placeholder, value, onChange }) => {
    return (
        <div className="bg-gray-100 p-2 rounded flex gap-x-2 items-center group border border-transparent transition-colors focus-within:border-primary">
            <SearchIcon className="w-5 h-5 text-gray-400" />
            <input className="flex-1 text-sm bg-transparent focus:outline-none placeholder:text-gray-400"
                placeholder={placeholder}
                maxLength={100}
                value={value} onChange={onChange} />
        </div>
    )
}


export default SearchInput
