import { useEffect, useRef, useState } from "react";
import Datepicker from "react-tailwindcss-datepicker";
import * as echarts from 'echarts';
import dayjs from 'dayjs';

export default function () {
    const now = dayjs()
    const [dateRange, setDateRange] = useState({
        startDate: now.add(-7, 'day').format('YYYY-MM-DD'),
        endDate: now.format('YYYY-MM-DD')
    });
    const chartRef = useRef<HTMLDivElement>(null)

    useEffect(() => {
        if (chartRef.current) {
            const chart: echarts.ECharts = echarts.init(chartRef.current);
            const option: echarts.EChartsOption = {
                tooltip: {
                    trigger: 'axis'
                },
                legend: {
                    data: ['总用户数', '登录用户数', '新增用户数'],
                    right: 10,
                    top: 10
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    boundaryGap: false,
                    data: ['2023-04-01', '2023-04-02', '2023-04-03', '2023-04-04', '2023-04-05', '2023-04-06', '2023-04-07']
                },
                yAxis: {
                    type: 'value'
                },
                series: [
                    {
                        name: '总用户数',
                        type: 'line',
                        stack: 'Total',
                        data: [120, 132, 101, 134, 90, 230, 210],
                        smooth: false
                    },
                    {
                        name: '登录用户数',
                        type: 'line',
                        stack: 'Total',
                        data: [220, 182, 191, 234, 290, 330, 310],
                        smooth: false
                    },
                    {
                        name: '新增用户数',
                        type: 'line',
                        stack: 'Total',
                        data: [150, 232, 201, 154, 190, 330, 410],
                        smooth: false
                    }
                ]
            };
            chart.setOption(option);

            const chartResize = () => chart.resize()

            window.addEventListener('resize', chartResize);

            return () => {
                window.removeEventListener('resize', chartResize)
            }
        }
    }, [])

    const handleValueChange = (newValue: any) => {
        console.log("newValue:", newValue);
        setDateRange(newValue);
    }

    return (
        <div className="grid gap-y-4">
            <div className="flex">
                <span className="flex-auto font-medium text-xl">时间趋势</span>
                <div className="flex-0 w-[240px] min-w-[200px]">
                    <Datepicker i18n="zh-cn"
                        configs={{}}
                        value={dateRange}
                        onChange={handleValueChange}
                        classNames={{
                            input: () => "text-center text-sm relative py-1.5 pl-4 pr-10 w-full border-gray-300 dark:bg-slate-800 dark:text-white/80 dark:border-slate-600 rounded-lg tracking-wide font-light placeholder-gray-400 bg-white focus:ring disabled:opacity-40 disabled:cursor-not-allowed focus:border-blue-500 focus:ring-blue-500/20"
                        }} />
                </div>
            </div>
            <div className="flex-auto w-full h-[400px]">
                <div className="w-full h-full" ref={chartRef}></div>
            </div>
        </div>
    )
}