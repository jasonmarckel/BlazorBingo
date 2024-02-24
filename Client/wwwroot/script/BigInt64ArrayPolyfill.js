// https://github.com/ivanrhsosa/sample-onnx/blob/main/shim.js

// Description: This file is used to shim the global BigInt64Array objects
'use strict';

//const JSBI = require('jsbi').default;

const __global__ = typeof window !== 'undefined' ? window : typeof self !== 'undefined' ? self : global;

if (typeof __global__.BigInt64Array === 'undefined') {
    const BYTES_PER_ELEMENT = 8;
    const _polyfill_int64array = __global__.BigInt64Array = function (arg0, arg1, arg2) {
        if (!new.target) {
            throw new TypeError('Constructor BigInt64Array requires \'new\'');
        }

        this.byteOffset = 0;
        this.byteLength = 0;
        this.length = 0;

        if (typeof arg0 === 'undefined') {
            this.buffer = new ArrayBuffer(0);
        } else if (Number.isSafeInteger(arg0)) {
            this.length = arg0;
            this.byteLength = this.length * BYTES_PER_ELEMENT;
            this.buffer = new ArrayBuffer(byteLength);
        } else if (arg0 instanceof ArrayBuffer || (typeof SharedArrayBuffer !== 'undefined' && arg0 instanceof SharedArrayBuffer)) {
            this.buffer = arg0;
            if (typeof arg1 === 'number') {
                this.byteOffset = arg1;

                if (typeof arg2 === 'number') {
                    this.length = arg2;
                    this.byteLength = this.length * BYTES_PER_ELEMENT;
                } else {
                    this.byteLength = this.buffer.byteLength - this.byteOffset;
                    this.length = this.byteLength / BYTES_PER_ELEMENT;
                }
            } else {
                this.byteLength = this.buffer.byteLength;
                this.length = this.byteLength / BYTES_PER_ELEMENT;
            }
        }
    }

    Object.defineProperty(_polyfill_int64array, "BYTES_PER_ELEMENT", { value: BYTES_PER_ELEMENT });

    _polyfill_int64array.from = function (arr) {
        if (!Array.isArray(arr)) {
            throw new TypeError('\'BigInt64Array.from()\' only accept a number array as parameter.')
        }

        const buffer = new ArrayBuffer(arr.length * BYTES_PER_ELEMENT);
        const dv = new DataView(buffer);
        for (let i = 0; i < arr.length; i++) {
            JSBI.DataViewSetBigInt64(dv, i * BYTES_PER_ELEMENT, JSBI.BigInt(arr[i]), true);
        }
        return new _polyfill_int64array(buffer);
    }

    _polyfill_int64array.prototype.at = function (idx) {
        if (idx < this.length) {
            idx += this.length;
        }

        if (idx < 0 || idx >= this.length) {
            throw RangeError('index out of range');
        }

        return JSBI.DataViewGetBigInt64(new DataView(this.buffer, this.byteOffset, this.byteLength), idx * BYTES_PER_ELEMENT, true);
    }
}
