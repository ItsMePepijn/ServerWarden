import { trigger, transition, style, query, group, animate } from "@angular/animations";

export const authSlideAnimation =
  trigger('routeAnimations', [
    transition('login => register', [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          position: 'absolute',
          top: 0,
          left: 0,
          padding: '32px',
          width: '100%'
        })
      ]),
      query(':enter', [
        style({ left: '100%' })
      ]),
      group([
        query(':leave', [
          animate('300ms ease-out', style({ left: '-100%'}))
        ]),
        query(':enter', [
          animate('300ms ease-out', style({ left: '0%' }))
        ]),
      ]),
    ]),
    transition('register => login', [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          position: 'absolute',
          top: 0,
          left: 0,
          padding: '32px',
          width: '100%'
        })
      ]),
      query(':enter', [
        style({ left: '-100%' })
      ]),
      group([
        query(':leave', [
          animate('300ms ease-out', style({ left: '100%'}))
        ]),
        query(':enter', [
          animate('300ms ease-out', style({ left: '0%' }))
        ]),
      ]),
    ]),
  ]);

export const slideAnimation =
  trigger('routeAnimations', [
    transition('home => server-details', [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          position: 'absolute',
          top: 0,
          left: 0,
          width: '100%',
        })
      ]),
      query(':enter', [
        style({ left: '100%' })
      ]),
      group([
        query(':leave', [
          animate('300ms ease-in-out', style({ left: '-100%'}))
        ]),
        query(':enter', [
          animate('300ms ease-in-out', style({ left: '0%' }))
        ]),
      ]),
    ]),
    transition('server-details => home', [
      style({ position: 'relative' }),
      query(':enter, :leave', [
        style({
          position: 'absolute',
          top: 0,
          left: 0,
          width: '100%'
        })
      ]),
      query(':enter', [
        style({ left: '-100%' })
      ]),
      group([
        query(':leave', [
          animate('300ms ease-in-out', style({ left: '100%'}))
        ]),
        query(':enter', [
          animate('300ms ease-in-out', style({ left: '0%' }))
        ]),
      ]),
    ]),
  ]);
