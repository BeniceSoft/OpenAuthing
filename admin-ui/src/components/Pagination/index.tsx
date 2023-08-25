import { ChevronLeftIcon, ChevronRightIcon } from '@heroicons/react/24/outline';
import React from 'react';

interface PaginationProps {
    pageIndex: number;
    pageSize: number;
    totalCount: number;
    onPageChange?: (pageIndex: number, pageSize?: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
    pageIndex,
    pageSize,
    totalCount,
    onPageChange = () => { },
}) => {
    const totalPages = Math.max(Math.ceil(totalCount / pageSize), 1);
    const prevPage = pageIndex > 1 ? pageIndex - 1 : null;
    const nextPage = pageIndex < totalPages ? pageIndex + 1 : null;

    const getPageIndexList = () => {
        const indexList = [];
        if (totalPages <= 5) {
            for (let i = 1; i <= totalPages; i++) {
                indexList.push(i);
            }
        } else {
            if (pageIndex <= 3) {
                console.log('pageindex <= 3')
                indexList.push(1, 2, 3, 4, -1, totalPages);
            } else if (pageIndex >= totalPages - 3) {
                indexList.push(1, -1, totalPages - 3, totalPages - 2, totalPages - 1, totalPages);
            } else {
                indexList.push(1, -1, pageIndex - 1, pageIndex, pageIndex + 1, -1, totalPages);
            }
        }

        return indexList;
    };

    const handlePageIndexChange = (newPageIndex: number) => {
        onPageChange(newPageIndex);
    };

    return (
        <nav className="flex items-center justify-between gap-x-2 text-sm text-gray-700 dark:text-gray-200">
            <span className="text-gray-600 text-xs dark:text-gray-400">共 {totalCount} 条</span>
            <div>
                <button
                    onClick={() => prevPage !== null && onPageChange(prevPage)}
                    disabled={prevPage === null}
                    className="w-8 h-8 flex items-center justify-center border border-transparent rounded hover:bg-gray-200 disabled:text-gray-400 disabled:hover:bg-transparent disabled:cursor-not-allowed"
                >
                    <ChevronLeftIcon className="w-4 h-4" />
                </button>
            </div>
            <div className="flex items-center space-x-2">
                {getPageIndexList().map((index, idx) => index === -1 ? (
                    <span key={idx}>...</span>
                ) : (
                    <button
                        key={idx}
                        onClick={() => handlePageIndexChange(index)}
                        className={`h-8 w-8 text-xs ${index === pageIndex
                            ? 'text-blue-600 border-blue-600'
                            : 'hover:bg-gray-200 dark:hover:bg-gray-400 border-transparent'
                            } border rounded`}
                    >
                        {index}
                    </button>
                ))}
            </div>
            <div>
                <button
                    onClick={() => nextPage !== null && onPageChange(nextPage)}
                    disabled={nextPage === null}
                    className="w-8 h-8 flex items-center justify-center border border-transparent rounded hover:bg-gray-200 disabled:text-gray-400 disabled:hover:bg-transparent disabled:cursor-not-allowed"
                >
                    <ChevronRightIcon className="w-4 h-4" />
                </button>
            </div>
        </nav>
    );
};

export default Pagination;