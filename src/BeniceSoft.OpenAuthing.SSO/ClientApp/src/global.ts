String.prototype.ensureStartsWith = function (prefix: string): string {
    return this.startsWith(prefix) ? this.toString() : prefix + this;
}