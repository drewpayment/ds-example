import { Component, OnInit, ViewEncapsulation, ChangeDetectionStrategy, AfterContentInit, OnDestroy, Optional, SkipSelf, Inject, ChangeDetectorRef, ViewContainerRef, Input, Output, EventEmitter, SimpleChanges, ContentChild, ViewChild, ElementRef, OnChanges, Directive } from '@angular/core';
import { matExpansionAnimations } from '@angular/material/expansion';
import { DS_EXPANSION_BASE, DsExpansionBase } from '@ds/core/ui/ds-expansion/ds-expansion-base';
import { UniqueSelectionDispatcher } from '@angular/cdk/collections';
import { DOCUMENT } from '@angular/common';
import { ANIMATION_MODULE_TYPE } from '@angular/platform-browser/animations';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { CdkAccordionItem } from '@angular/cdk/accordion';
import { Subject } from 'rxjs';
import { DsExpansionContentDirective } from '@ds/core/ui/ds-expansion/ds-expansion-content.directive';
import { TemplatePortal } from '@angular/cdk/portal';
import { startWith, filter, take } from 'rxjs/operators';
import { AnimationEvent } from '@angular/animations';

// TODO:(devversion): workaround for https://github.com/angular/material2/issues/12760
export const _CdkAccordionItem = CdkAccordionItem;

export type DsExpansionPanelState = 'expanded' | 'collapsed';
let uniqueId = 0;

@Component({
    selector: 'ds-expansion',
    exportAs: 'dsExpansion',
    templateUrl: './ds-expansion.component.html',
    styleUrls: ['./ds-expansion.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    inputs: ['disabled', 'expanded'],
    outputs: ['opened', 'closed', 'expandedChange'],
    animations: [matExpansionAnimations.bodyExpansion],
    providers: [{ provide: DS_EXPANSION_BASE, useValue: undefined }],
    host: {
        'class': 'ds-card-collapsible',
        '[class.open]': 'expanded'
    }
})
export class DsExpansionComponent extends CdkAccordionItem implements AfterContentInit, OnChanges, OnDestroy {
    private _document:Document | undefined;
    accordion:DsExpansionBase;
    private _hideToggle:boolean = false;
    @Input()
    get hideToggle():boolean {
        return this._hideToggle || (this.accordion && this.accordion.hideToggle);
    }
    set hideToggle(value:boolean) {
        this._hideToggle = coerceBooleanProperty(value);
    }

    @Output() afterExpand = new EventEmitter<void>();
    @Output() afterCollapse = new EventEmitter<void>();
    readonly _inputChanges = new Subject<SimpleChanges>();
    @ContentChild(DsExpansionContentDirective, { static: true }) _lazyContent:DsExpansionContentDirective;
    @ViewChild('body', { static: true }) _body:ElementRef<HTMLElement>;
    _portal:TemplatePortal;
    _headerId = `ds-expansion-panel-header-${uniqueId++}`;

    constructor(
        @Optional() @SkipSelf() @Inject(DS_EXPANSION_BASE) accordion:DsExpansionBase,
        _changeDetectorRef:ChangeDetectorRef,
        _uniqueSelectionDispatcher:UniqueSelectionDispatcher,
        private _viewContainerRef:ViewContainerRef,
        @Inject(DOCUMENT) _document?:any,
        @Optional() @Inject(ANIMATION_MODULE_TYPE) public _animationMode?:string
    ) { 
        super(accordion, _changeDetectorRef, _uniqueSelectionDispatcher);
        this.accordion = accordion;
        this._document = _document;
    }

    ngOnInit() {
    }

    ngAfterContentInit() {
        if(this._lazyContent) {
            this.opened.pipe(
                startWith<void>(null!),
                filter(() => this.expanded && !this._portal),
                take(1)
            ).subscribe(() => {
                this._portal = new TemplatePortal(this._lazyContent._template, this._viewContainerRef);
            });
        }
    }

    ngOnChanges() {
        super.ngOnDestroy();
        this._inputChanges.complete();
    }

    _getExpandedState():DsExpansionPanelState {
        return this.expanded ? 'expanded' : 'collapsed';
    }

    _bodyAnimation(event:AnimationEvent) {
        const classList = event.element.classList;
        const cssClass = 'open';
        const {phaseName, toState, fromState} = event;

        if(phaseName === 'done' && toState === 'expanded') {
            classList.add(cssClass);
        }
        if(phaseName === 'start' && toState === 'collapsed') {
            classList.remove(cssClass);
        }

        if(phaseName === 'done' && toState === 'expanded' && fromState !== 'void') {
            this.afterExpand.emit();
        }
        if(phaseName === 'done' && toState === 'collapsed' && fromState !== 'void') {
            this.afterCollapse.emit();
        }
    }

    _containsFocus():boolean {
        if(this._body && this._document) {
            const focusedElement = this._document.activeElement;
            const bodyElement = this._body.nativeElement;
            return focusedElement === bodyElement || bodyElement.contains(focusedElement);
        }
        return false;
    }

}

@Directive({
    selector: 'ds-action-row',
    host: {
        class: 'ds-action-row'
    }
})
export class DsExpansionPanelActionRow {}