// @ts-nocheck
// This file is generated by Umi automatically
// DO NOT CHANGE IT MANUALLY!
// It's faked dva
// aliased to @umijs/plugins/templates/dva
import { create, Provider } from 'dva';
import createLoading from '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/node_modules/dva-loading/dist/index.js';

import React, { useRef } from 'react';
import { history, ApplyPluginsType, useAppData } from 'umi';
import { models } from './models';

let dvaApp: any;

export function RootContainer(props: any) {
  const { pluginManager } = useAppData();
  const app = useRef<any>();
  const runtimeDva = pluginManager.applyPlugins({
    key: 'dva',
    type: ApplyPluginsType.modify,
    initialValue: {},
  });
  if (!app.current) {
    app.current = create(
      {
        history,
        ...(runtimeDva.config || {}),
      },
      {
        initialReducer: {},
        setupMiddlewares(middlewares: Function[]) {
          return [...middlewares];
        },
        setupApp(app: IDvaApp) {
          app._history = history;
        },
      },
    );
    dvaApp = app.current;
    app.current.use(createLoading());
    
    
    
    (runtimeDva.plugins || []).forEach((p) => {
      app.current.use(p);
    });
    for (const id of Object.keys(models)) {
      app.current.model({
        namespace: models[id].namespace,
        ...models[id].model,
      });
    }
    app.current.start();
  }
  return <Provider store={app.current!._store}>{props.children}</Provider>;
}

export function getDvaApp() {
  return dvaApp;
}
