// @ts-nocheck
// This file is generated by Umi automatically
// DO NOT CHANGE IT MANUALLY!
import * as Plugin_0 from '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/app.ts';
import * as Plugin_1 from '@@/core/helmet.ts';
import * as Plugin_2 from '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/.umi/plugin-initialState/runtime.tsx';
import * as Plugin_3 from '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/.umi/plugin-model/runtime.tsx';
import * as Plugin_4 from '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/.umi/plugin-dva/runtime.tsx';
import * as Plugin_5 from '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/.umi/plugin-qiankun-slave/slaveRuntimePlugin.ts';
import { PluginManager } from 'umi';

function __defaultExport (obj) {
  if (obj.default) {
    return typeof obj.default === 'function' ? obj.default() :  obj.default
  }
  return obj;
}
export function getPlugins() {
  return [
    {
      apply: __defaultExport(Plugin_0),
      path: process.env.NODE_ENV === 'production' ? void 0 : '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/app.ts',
    },
    {
      apply: Plugin_1,
      path: process.env.NODE_ENV === 'production' ? void 0 : '@@/core/helmet.ts',
    },
    {
      apply: Plugin_2,
      path: process.env.NODE_ENV === 'production' ? void 0 : '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/.umi/plugin-initialState/runtime.tsx',
    },
    {
      apply: Plugin_3,
      path: process.env.NODE_ENV === 'production' ? void 0 : '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/.umi/plugin-model/runtime.tsx',
    },
    {
      apply: Plugin_4,
      path: process.env.NODE_ENV === 'production' ? void 0 : '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/.umi/plugin-dva/runtime.tsx',
    },
    {
      apply: Plugin_5,
      path: process.env.NODE_ENV === 'production' ? void 0 : '/Users/andezeng/Codes/BeniceSoft/OpenAuthing/admin-ui/src/.umi/plugin-qiankun-slave/slaveRuntimePlugin.ts',
    },
  ];
}

export function getValidKeys() {
  return ['patchRoutes','patchClientRoutes','modifyContextOpts','modifyClientRenderOpts','rootContainer','innerProvider','i18nProvider','accessProvider','dataflowProvider','outerProvider','render','onRouteChange','getInitialState','dva','request','qiankun',];
}

let pluginManager = null;

export function createPluginManager() {
  pluginManager = PluginManager.create({
    plugins: getPlugins(),
    validKeys: getValidKeys(),
  });


  return pluginManager;
}

export function getPluginManager() {
  return pluginManager;
}