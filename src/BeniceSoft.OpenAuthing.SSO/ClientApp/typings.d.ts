import 'umi/typings';

declare global {
    interface String {
        ensureStartsWith(prefix: string): string
    }
}