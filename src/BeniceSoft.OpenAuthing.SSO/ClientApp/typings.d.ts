import 'umi/typings';
import "preline/preline";
import { IStaticMethods } from "preline/preline";

declare global {
    interface String {
        ensureStartsWith(prefix: string): string
    }
    interface Window {
        HSStaticMethods: IStaticMethods;
    }
}