import { LocationStrategy } from "@angular/common";
import { LocalLinkService } from "./local-link.service";
import { Injectable, OnDestroy } from "@angular/core";
import { ConfigUrl } from "@ds/core/shared/config-url.model";
import { Subject } from "rxjs";

@Injectable()
export class WrappedLocationStrategy implements LocationStrategy, OnDestroy {
  private sites: ConfigUrl[];
  destroy$ = new Subject();

  constructor(
    private loc: LocationStrategy,
    private localLinkService: LocalLinkService
  ) {}

  ngOnDestroy() {
    this.destroy$.next();
  }

  path(includeHash?: boolean): string {
    return this.loc.path(includeHash);
  }

  prepareExternalUrl(internal: string) {
    return this.localLinkService.baseHref;
  }

  pushState(state: any, title: string, url: string, queryParams: string): void {
    return this.loc.pushState(state, title, url, queryParams);
  }

  replaceState(
    state: any,
    title: string,
    url: string,
    queryParams: string
  ): void {
    return this.loc.replaceState(state, title, url, queryParams);
  }

  forward(): void {
    return this.loc.forward();
  }

  back(): void {
    return this.loc.back();
  }

  onPopState(fn: any): void {
    return this.loc.onPopState(fn);
  }

  getBaseHref(): string {
    return this.localLinkService.baseHref;
  }
}
