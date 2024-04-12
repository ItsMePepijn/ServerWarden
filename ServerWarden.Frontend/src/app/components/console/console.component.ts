import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';

@Component({
  selector: 'app-console',
  templateUrl: './console.component.html',
  styleUrl: './console.component.scss'
})
export class ConsoleComponent implements AfterViewInit{
  @Input() public title: string = "Console";
  @Input() public logLines: string[] = [];
  @Input() public ableToInput: boolean = true;

  @ViewChild('consoleWindow') consoleWindow: any;

  constructor() {
  }

  ngAfterViewInit() {
    let observer = new MutationObserver(() => {
      this.consoleWindow.nativeElement.scrollTop = this.consoleWindow.nativeElement.scrollHeight;
    });

    observer.observe(this.consoleWindow.nativeElement, { childList: true });
  }

  public input: string = "";

  public sendCommand(): void {
    if(this.input.length === 0) return;

    this.logLines.push(this.input);
    this.input = "";
  }
}
