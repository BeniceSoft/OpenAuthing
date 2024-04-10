import { IApi } from 'umi';

export default (api: IApi) => {
    api.describe({
        key: 'globalScrollbar',
        config: {
            schema(joi) {
                return joi.object({

                })
            },
        },
        enableBy: api.EnableBy.config
    });

    api.addHTMLStyles(() => {

        return `
            /* width */
            ::-webkit-scrollbar {
                width: 10px;
                height: 10px;
            }
            
            /* Track */
            ::-webkit-scrollbar-track {
                background: transparent;
                border-radius: 5px;
            }
            
            /* Handle */
            ::-webkit-scrollbar-thumb {
                background: #efefee;
                border-radius: 5px;
            }
            
            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #cccccc;
            }
        `
    })
};