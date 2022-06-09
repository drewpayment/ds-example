import { Component, OnInit, Input, HostBinding } from '@angular/core';

@Component({
    selector: 'ds-progress',
    templateUrl: './ds-progress.component.html',
    styleUrls: ['./ds-progress.component.scss'],
    host: {
        'class': 'ds-progress-bar',
        '[class.progress-primary]': "!this.color || this.color === 'primary'",
        '[class.progress-info]': "this.color === 'info'",
        '[class.progress-warning]': "this.color === 'warning'",
        '[class.progress-danger]': "this.color === 'danger'",
        '[class.progress-success]': "this.color === 'success'",
        '[class.sm]': "this.size === 'sm' || this.size === 'small'",
        '[class.top]': "this.labelPosition === 'top'",
        '[class.right]': "this.labelPosition === 'right'",
        '[class.left]': "this.labelPosition === 'left'"
    }
})
export class DsProgressComponent implements OnInit {

    @Input()
    value: number;
    
    @Input()
    bufferValue: number;

    @Input()
    mode?: "determinate" | "indeterminate" | "buffer" | "query";

    @Input()
    color?: "info" | "success" | "warning" | "primary" | "danger";

    @Input()
    size?: "small" | "sm" | "medium" | "md";

    @Input()
    labelPosition?: "left" | "right" | "top";

    @Input()
    label?: string;

    constructor() { }

    ngOnInit() {
    }
}
