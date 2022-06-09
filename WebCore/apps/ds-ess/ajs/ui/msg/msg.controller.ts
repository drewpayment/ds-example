export class EssMsgController {
  static readonly CONTROLLER_NAME = "essMsgController";
  static readonly CONFIG = {
    controller: EssMsgController,
    template: require('./msg.html'),
    bindings: {},
    controllerAs: '$ctrl'
  };
  static readonly $inject = [  ];

  constructor(  ) {
  }
}
