import { Directive, EmbeddedViewRef, Input, TemplateRef, ViewContainerRef, ɵstringify as stringify } from '@angular/core';

@Directive({
  selector: '[not]',
})
export class NotDirective {
  private _context: NotContext<boolean> = new NotContext<boolean>();
  private _thenTemplateRef: TemplateRef<NotContext<boolean>>|null = null;
  private _thenViewRef: EmbeddedViewRef<NotContext<boolean>>|null = null;

  constructor(
    templateRef: TemplateRef<NotContext<boolean>>,
    private vc: ViewContainerRef,
  ) {
    this._thenTemplateRef = templateRef;
  }

  @Input() set not(check: boolean) {
    this._context.$implicit = this._context.not = !check;
    this._updateView();
  }

  private _updateView() {
    if (this._context.$implicit) {
      if (!this._thenViewRef) {
        this.vc.clear();

        if (this._thenTemplateRef) {
          this._thenViewRef = this.vc.createEmbeddedView(this._thenTemplateRef, this._context);
        }
      }
    }
  }

}

export class NotContext<T = any> {
  public $implicit: T = null!;
  public not: T = null!;
}

