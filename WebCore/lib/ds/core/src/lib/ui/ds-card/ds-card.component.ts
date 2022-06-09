import { Component, Directive, ViewEncapsulation, ChangeDetectionStrategy, Output, EventEmitter, Input, OnInit, HostBinding, AfterContentInit, ComponentRef, ElementRef, TemplateRef, Renderer2, ComponentFactoryResolver, OnDestroy, OnChanges, SimpleChanges, Self } from '@angular/core';
import { DsExpansionService } from '@ds/core/ui/ds-expansion/ds-expansion.service';
import { Subscription, Observable } from 'rxjs';
import { DsCardService } from '@ds/core/ui/ds-card/ds-card.service';
import { coerceBooleanProperty } from '@angular/cdk/coercion';

const _collapse = false;

export const BOOTSTRAP_COLORS = [
    'primary',
    'secondary',
    'success',
    'danger',
    'warning',
    'info',
    'light',
    'dark',
    'body',
    'muted',
    'white',
    'black-50',
    'white-50'
];

type CardMode = 'card' | 'widget' | 'callout' | 'nobody' | 'widget-nobody' | 'noheader' | 'object' | 'nav' | 'stepper' | 'footer';

@Component({
    selector: 'ds-card',
    exportAs: 'dsCard',
    templateUrl: './ds-card.component.html',
    styleUrls: ['./ds-card.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [DsExpansionService, DsCardService],
    host: {
        // '[class.active]': 'drawerMaskActive',
        // '[class.give-me-some-space]': 'collapse === false'
    }
})
export class DsCardComponent implements OnInit, OnChanges, OnDestroy, AfterContentInit {
    private _border: string;
    /**
     * This does not follow typical JavaScript truthiness rules (i.e. 0 evaluates to true).
     * @see https://netbasal.com/trust-but-verify-coerce-your-component-inputs-bdb743e8b579 (read the section about coerceBooleanProperty)
     */
    @Input() collapse: boolean;
    @Input('mode')
    set mode(value: CardMode) {
        if ( value !== 'card' && value !== 'widget' && value !== 'callout' && value !== 'nobody' && value !== 'widget-nobody' && value !== 'noheader' && value !== 'object' && value !== 'nav' && value !== 'stepper' && value !== 'footer' )
            this._mode = 'card';
        else
            this._mode = value;
    }
    get mode(): CardMode {
        return this._mode;
    }
    @Input('color')
    set color(value: string) {
        if (value !== 'primary' && value !== 'secondary' && value !== 'danger' && value !== 'warning' &&
            value !== 'info' && value != 'yellow' && value !== 'light' && value !== 'dark' && value !== 'success' &&
            value !== 'form-danger' && value !== 'gray' && value !== 'medium' &&
            value !== 'medium-dark' && value !== 'super-dark-gray' && value !== 'info-special' && value !== 'archive' &&
            value !== 'disabled' && value !== 'navy' && value != 'default-blue' && value != 'add') {
                this._color = 'primary';
        } else {
            this._color = value;
        }
    }
    get color(): string {
        return this._color;
    }

    @Input('color-fade') colorFade: boolean;
    @Input() expanded: boolean;

    /**
     * Handle dynamic class assignment
     * see: https://github.com/angular/angular/iss ues/7289#issuecomment-390415535
     */
    @Input('class') origClasses = '';
    @HostBinding('class')
    get hostClasses(): string {
        return [
            this.origClasses,
            'ds-card',
            this.resolveCardModeClass() ? `${this._cardModeClass}` : '',
            this._color ? `${this._color}` : '',
            this.border != null ? `${this.border}` : '',
            this.hover ? 'action' : '',
            this.drawerMaskActive ? 'active' : '',
            this.collapse === false ? 'no-expansion-switch-placeholder' : '',
            this.matchHeight ? 'card-height-fix-sm' : ''
        ].join(' ');
    }

    private _color: string;
    private _mode: CardMode;
    private _cardModeClass: string;
    private _expanded: boolean;
    private _sub: Subscription;
    private _hover: boolean;

    // has to be below the getter/setter for color
    @Input('border')
    set border(value: string|string[]) {
        if (Array.isArray(value)) {
            const arrBorder = 'header-bar';
            for (let i = 0; i < value.length; i++) {
                arrBorder.concat(` border-${value[i]}`);
            }
            this._border = arrBorder;
        } else if (value == null) {
            this._border = '';
        } else {
            this._border = `header-bar border-${value}`;
        }
    }
    get border(): string|string[] {
        return this._border;
    }

    @Input()
    get hover(): boolean {
        return this._hover;
    }
    set hover(value: boolean) {
        this._hover = coerceBooleanProperty(value);
    }
    private _drawerMaskActive = false;
    @Input()
    get drawerMaskActive(): boolean {
        return this._drawerMaskActive;
    }
    set drawerMaskActive(value: boolean) {
        this._drawerMaskActive = coerceBooleanProperty(value);
    }
    @Input() drawerMask = false;
    @Input() matchHeight = false;
    constructor(private service: DsExpansionService, private cardService: DsCardService) {
        this._sub = this.service.expanded$.subscribe(next => this._expanded = next);
    }
    ngOnInit() {
        this.service.setCollapse(this.collapse);
        this.cardService.setTruncate(this.mode === 'callout' || this.mode === 'nobody');
    }
    ngAfterContentInit() {
    }
    ngOnChanges(changes: SimpleChanges) {
        if (changes.expanded)
            this.service.toggle(!!changes.expanded.currentValue);
        if (changes.collapse)
            this.service.setCollapse(changes.collapse.currentValue);

    }
    ngOnDestroy() {
        this._sub.unsubscribe();
    }
    // toggleExpansion(isExpanded:boolean) {
    //     console.log('The panel is expanded? ' + isExpanded);
    // }
    private resolveCardModeClass(): boolean {
        this._cardModeClass =
              this._mode === 'card' ? ''
            : this._mode === 'widget' || this._mode === 'widget-nobody' ? 'ds-card-widget'
            : this._mode === 'callout' ? 'ds-card-callout'
            : this._mode === 'noheader' ? 'noheader'
            : this._mode === 'nobody' ? 'card-nobody'
            : this._mode === 'object' ? 'ds-object widget-nobody'
	          : this._mode === 'nav' ? 'navigation-card'
            : this._mode === 'stepper' ? 'stepper-card'
            : this._mode === 'footer' ? 'footer-card'
            : '';
        return this._cardModeClass != null;
    }
}

@Component({
    selector: 'ds-card-header, [ds-card-header], [dsCardHeader]',
    templateUrl: './ds-card-header.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.Default,
    host: {
        '[class.ds-card-header]': 'true',
        '[class.align-x-center]': 'this.hAlign == "center"',
        '[class.text-center]': 'this.hAlign == "center"',
        '[class.d-block]': 'true',
        '[class.align-y-top]': 'this.vAlign == "top"',
        '[class.align-y-center]': 'this.vAlign == "center"',
        '[class.align-y-bottom]': 'this.vAlign == "bottom"',
        '[class.collapse]': 'this.collapse == true'
        // '[class.align-center]': 'this.hAlign == "center" && this.vAlign == "center"'
    }
})
export class DsCardHeader implements OnDestroy {
    @Input('x-align') hAlign: 'left' | 'center' | 'right' = 'left';
    @Input('y-align') vAlign: 'center' | 'top' | 'bottom' = 'center';
    collapse: boolean;
    expanded = false;
    @Output() expansionChange = new EventEmitter<boolean>();

    private _subscription: Subscription;
    private _collapseSubscription: Subscription;

    headerBorderClass: string = null;
    constructor(private service: DsExpansionService) {
        this._subscription = this.service.expanded$.subscribe(next => this.expanded = next);
        this._collapseSubscription = this.service.collapse$.subscribe(next => this.collapse = next);
    }
    ngOnDestroy() {
        this._subscription.unsubscribe();
        this._collapseSubscription.unsubscribe();
    }

    handleExpansionChange() {
        this.expanded = !this.expanded;
        this.service.toggle(this.expanded);
        this.expansionChange.emit(this.expanded);
    }
}

@Directive({
    selector: 'ds-card-precontent, [ds-card-precontent], [dsCardPrecontent]',
    host: {'class': 'ds-card-precontent'}
})
export class DsCardPrecontent {}

@Directive({
    selector: 'ds-card-icon-container, [ds-card-icon-container], [dsCardIconContainer]',
    host: {'class': 'ds-card-icon-container'}
})
export class DsCardIconContainer {}

@Directive({
    selector: 'ds-card-subtitle, [ds-card-subtitle], [dsCardSubtitle]',
    host: {
        '[class.ds-card-subtitle]': 'true'
    }
})
export class DsCardSubtitle {}

@Directive({
    selector: 'ds-card-nav, [ds-card-nav], [dsCardNav]',
    host: {
        '[class.ds-card-nav]': 'true'
    }
})

export class DsCardNav {}

// Legacy support only -- Adds left gray border
@Directive({
    selector: 'ds-card-title',
    host: {'class': 'card-title'}
})
export class DsCardTitle {}

@Directive({
    selector: 'ds-card-icon-label, [ds-card-icon-label], [dsCardIconLabel]',
    host: {
        '[class.ds-card-icon-container]': 'true',
        // '[class.align-header]': 'false',
        '[class.align-center]': 'this.vAlign == "center"',
        '[class.w-lg]': 'true'
    }
})
export class DsCardIconLabel implements AfterContentInit {
    @Input('v-align') vAlign: 'header' | 'center';

    constructor(private service: DsExpansionService) {}
    ngAfterContentInit() {
        this.service.setHasCardIcon(true);
    }
}

@Component({
    selector: 'div[ds-card-icon], div[dsCardIcon]',
    encapsulation: ViewEncapsulation.Emulated,
    changeDetection: ChangeDetectionStrategy.OnPush,
    template: `
        <i [class.md-36]="size == 'lg'" [class]="isOutline ? 'material-icons-outlined' : 'material-icons'"><ng-content></ng-content></i>
        <ng-content select="ds-card-icon-label, [ds-card-icon-label], [dsCardIconLabel]"></ng-content>
    `,
    host: {
        '[class.ds-card-icon]': 'true',
        '[class.align-header]': 'true',
        '[class.w-lg]': 'this.size == "lg"'
    }
})
export class DsCardIcon implements AfterContentInit {
    @Input('size') size: 'sm' | 'lg';
    @Input('isOutline') isOutline: Boolean;

    constructor(private service: DsExpansionService) {

    }
    ngAfterContentInit() {
        this.service.setHasCardIcon(true);
    }
}

// Legacy support only -- Adds left gray border
@Directive({
  selector: 'ds-card-avatar'
})
export class DsCardAvatar {}

@Directive({
    selector: 'ds-card-widget-title',
    host: {
      '[class.font-xl]': 'true',
      '[class.text-truncate]': 'true'
    }
})
export class DsCardWidgetTitle {}

@Directive({
    selector: 'ds-card-header-title',
    host: {
        '[class.h1]': 'true'
    }
})
export class DsCardHeaderTitle {
    @HostBinding('class.text-truncate') truncateClass: boolean;

    constructor(private cardService: DsCardService) {
        this.cardService.truncate$.subscribe(truncate => {
            this.truncateClass = truncate;
        });
    }
}

@Directive({
  selector: 'ds-card-bread-crumb, [ds-card-bread-crumb], [dsCardBreadCrumb',
  host: {
    '[class.ds-card-breadcrumb]': 'true'
  }
})
export class DsCardBreadCrumb {
  constructor() {}
}

@Directive({
    selector: 'ds-card-sub-header-title, [ds-card-sub-header-title], [dsCardSubHeaderTitle]',
    host: {
        '[class.h2]': 'true'
    }
})
export class DsCardSubHeaderTitle {
    @HostBinding('class.text-truncate') truncateClass: boolean;

    constructor(private cardService: DsCardService) {
        this.cardService.truncate$.subscribe(truncate => {
            this.truncateClass = truncate;
        });
    }
}

@Directive({
    selector: 'ds-card-section-title',
    host: {
        '[class.h3]': 'true'
    }
})
export class DsCardSectionTitle {
    @HostBinding('class.text-truncate') truncateClass: boolean;

    constructor(private cardService: DsCardService) {
        this.cardService.truncate$.subscribe(truncate => {
            this.truncateClass = truncate;
        });
    }
}

@Component({
    selector: 'ds-card-title-icon',
    template: `
        <div class="ds-card-title-icon">
            <i class="material-icons" [ngClass]="textColor"><ng-content></ng-content></i>
        </div>
    `,
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    inputs: ['color']
})
export class DsCardTitleIcon implements OnInit {
    @Input() color: string;
    textColor: string;
    constructor() {}
    ngOnInit() {
        const isValidColor = this.color != null ? BOOTSTRAP_COLORS.indexOf(this.color) > -1 : -1;

        if (!isValidColor)
            this.color = 'body';

        this.textColor = 'text-' + this.color;
    }
}

@Directive({
    selector: 'ds-card-inline-content, [ds-card-inline-content], [dsCardInlineContent]',
    host: {'class': 'ds-card-inline-content'}
})
export class DsCardInlineContent {}

@Directive({
    selector: 'ds-card-title-right-content, [ds-card-title-right-content], [dsCardTitleRightContent]',
    host: {'class': 'ds-right-content'}
})
export class DsCardTitleRightContent {}

@Directive({
    selector: 'ds-card-title-action, [ds-card-title-action], [dsCardTitleAction]',
    host: {'class': 'ds-card-action'}
})
export class DsCardTitleAction implements OnDestroy {
    private _collapseSubscription: Subscription;
    collapse: boolean;
    constructor(private elRef: ElementRef, private service: DsExpansionService) {
        this._collapseSubscription = this.service.collapse$.subscribe(next => {
            this.collapse = next;
            if (this.collapse)
                this.elRef.nativeElement.classList.add('d-flex');
        });
    }

    ngOnDestroy() {
        this._collapseSubscription.unsubscribe();
    }
}

@Component({
    selector: 'ds-card-content, [ds-card-content]',
    exportAs: 'dsCardContent',
    templateUrl: './ds-card-content.html',
    host: {'class': 'ds-card-content'}
})
export class DsCardContent implements AfterContentInit, OnDestroy {
    // collapse:boolean;
    collapse = false;
    expanded = false;
    @Output() opened = new EventEmitter<void>();
    @Output() closed = new EventEmitter<void>();
    @Output() destroyed = new EventEmitter<void>();
    userContent: string;
    private _sub: Subscription;
    private _collapseSubscription: Subscription;
    constructor(private service: DsExpansionService) {
        this._sub = this.service.expanded$.subscribe(next => this.expanded = next);
        this._collapseSubscription = this.service.collapse$.subscribe(next => this.collapse = next);
    }
    ngAfterContentInit() {}
    ngOnDestroy() {
        this._sub.unsubscribe();
        this._collapseSubscription.unsubscribe();
    }
    handleOpened() { this.service.open(); }
    handleClosed() { this.service.close(); }
    handleDestroyed() { this.destroyed.emit(); }
}

@Directive({
    selector: 'ds-card-collapse-instruction-text, [ds-card-collapse-instruction-text], [dsCardCollapseInstructionText]',
    host: {'class': 'instruction-text'}
})
export class DsCardCollapseInstructionText {}

@Directive({
    selector: 'ds-card-footer, [ds-card-footer], [dsCardFooter]',
    host: {'class': 'ds-card-footer'}
})
export class DsCardFooter {}
