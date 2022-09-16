(function(){"use strict";var e={5190:function(e,t,r){r(6992),r(8674),r(9601),r(7727);var o=r(8935),n=r(9018),a=r.n(n),s=r(2196),u=r.n(s),l=r(6166),i=r.n(l),c=r(6762),p=r(6016),d=r.n(p),m=(r(44),r(8262)),f=r(3266),v=function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("div",{attrs:{id:"app"}},[r("b-alert",{attrs:{variant:"danger",dismissible:"",fade:"",show:e.error},on:{dismissed:function(t){e.error=!1}}},[e._v(" "+e._s(e.errorMessage)+" ")]),e.hasGroupData?r("form",{on:{submit:function(t){return t.preventDefault(),e.submitForm.apply(null,arguments)}}},[e._l(e.groupData,(function(t){return r("div",{key:t.name,staticClass:"form-group row"},[r("label",{staticClass:"col-md-2 col-form-label",attrs:{for:t.name}},["checkbox"!==t.type?r("p",[e._v(" "+e._s(t.label)+" ")]):e._e()]),r("div",{staticClass:"col-md-10"},["password"===t.type?[r("input",{directives:[{name:"model",rawName:"v-model.trim",value:t.value,expression:"group.value",modifiers:{trim:!0}}],staticClass:"form-control input-password",attrs:{id:t.name,type:t.type,required:t.required,name:t.name},domProps:{value:t.value},on:{input:function(r){r.target.composing||e.$set(t,"value",r.target.value.trim())},blur:function(t){return e.$forceUpdate()}}}),e._m(0,!0)]:e._e(),"text"===t.type?[r("input",{directives:[{name:"model",rawName:"v-model.trim",value:t.value,expression:"group.value",modifiers:{trim:!0}}],staticClass:"form-control",attrs:{id:t.name,type:t.type,required:t.required,name:t.name},domProps:{value:t.value},on:{input:function(r){r.target.composing||e.$set(t,"value",r.target.value.trim())},blur:function(t){return e.$forceUpdate()}}})]:e._e(),"textarea"===t.type?[r("textarea",{directives:[{name:"model",rawName:"v-model.trim",value:t.value,expression:"group.value",modifiers:{trim:!0}}],staticClass:"form-control",attrs:{id:t.name,required:t.required,name:t.name},domProps:{value:t.value},on:{input:function(r){r.target.composing||e.$set(t,"value",r.target.value.trim())},blur:function(t){return e.$forceUpdate()}}})]:e._e(),"checkbox"===t.type?[r("b-form-checkbox",{attrs:{id:t.name,name:t.name},model:{value:t.value,callback:function(r){e.$set(t,"value",r)},expression:"group.value"}},[e._v(" "+e._s(t.label)+" ")])]:e._e(),"select"===t.type?[r("multiselect",{attrs:{options:t.options,searchable:!1,"show-labels":!1,label:"label","track-by":"label"},model:{value:t.selectedValue,callback:function(r){e.$set(t,"selectedValue",r)},expression:"group.selectedValue"}})]:e._e(),"selectsearch"===t.type?[r("multiselect",{attrs:{options:t.options,"custom-label":e.nameWithLang,label:"label","track-by":"label"},model:{value:t.value,callback:function(r){e.$set(t,"value",r)},expression:"group.value"}})]:e._e(),"multiselect"===t.type?[r("multiselect",{attrs:{options:t.options,multiple:!0,"close-on-select":!1,"preserve-search":!0,label:"label","track-by":"label","reset-after":!1,"group-values":"groupOptions","group-label":"groupLabel","group-select":!0,"hide-selected":!0},model:{value:t.selectedValues,callback:function(r){e.$set(t,"selectedValues",r)},expression:"group.selectedValues"}})]:e._e()],2)])})),r("div",{staticClass:"mb-3 row"},[r("label",{staticClass:"col-md-2 col-form-label"},[e._v(" ")]),r("div",{staticClass:"col-md-10"},[r("button",{staticClass:"btn btn-primary pmt-2",attrs:{type:"submit"},on:{click:function(t){return t.preventDefault(),e.submitForm.apply(null,arguments)}}},[e._v(" Запиши ")])])])],2):e._e()],1)},b=[function(){var e=this,t=e.$createElement,r=e._self._c||t;return r("a",{staticClass:"show-pass",attrs:{href:"javascript:void(0)"}},[r("i",{staticClass:"fa fa-eye-slash"})])}],g=r(6198),h=(r(8975),{name:"App",data:function(){return{groupData:[],error:!1,errorMessage:""}},computed:{hasGroupData:function(){return 0!==this.groupData.length}},mounted:function(){this.getData()},methods:{getData:function(){var e=this;return(0,g.Z)(regeneratorRuntime.mark((function t(){var r,o,n;return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return r=e.$lodash.isEmpty(window.jsonData)?{jsonUrlControls:"https://corporate.orak365.net/SmartRegistry/UserGroups/UserGroupData"}:window.jsonData,t.prev=1,t.next=4,e.axios.get(r.jsonUrlControls);case 4:o=t.sent,n=o.data,console.log("getData",n),e.groupData=n,t.next=16;break;case 10:t.prev=10,t.t0=t["catch"](1),console.error("Error loading data from the server:",t.t0),e.error=!0,e.errorMessage=t.t0.errorMessage,setTimeout((function(){return e.resetError()}),5e3);case 16:case"end":return t.stop()}}),t,null,[[1,10]])})))()},submitForm:function(){var e=this;return(0,g.Z)(regeneratorRuntime.mark((function t(){var r,o,n;return regeneratorRuntime.wrap((function(t){while(1)switch(t.prev=t.next){case 0:return t.prev=0,e.resetError(),console.log("submit",e.groupData),r=e.$lodash.isEmpty(window.jsonData)?{}:window.jsonData,t.next=6,e.axios.post(r.jsonUrlCreateEdit,e.groupData);case 6:o=t.sent,n=o.data,"Error"===n.status||"error"===n.status?(e.error=!0,e.errorMessage=n.errorMessage,setTimeout((function(){return e.resetError()}),5e3)):"Success"!==n.status&&"success"!==n.status||location.assign(n.redirectUrl),t.next=17;break;case 11:t.prev=11,t.t0=t["catch"](0),console.error(t.t0),console.log("Error submitting form","\n",t.t0),e.$lodash.isEmpty(t.t0.errorMessage)||(e.error=!0,e.errorMessage=t.t0.errorMessage),setTimeout((function(){return e.resetError()}),5e3);case 17:case"end":return t.stop()}}),t,null,[[0,11]])})))()},resetError:function(){this.error=!1,this.errorMessage=""}}}),y=h,w=r(1001),x=(0,w.Z)(y,v,b,!1,null,null,null),_=x.exports;o["default"].use(a(),{name:"$lodash",lodash:u()}),o["default"].use(c.Z,i()),o["default"].use(m.XG7),o["default"].use(f.A7),o["default"].component("Multiselect",d()),o["default"].config.productionTip=!1,new o["default"]({render:function(e){return e(_)}}).$mount("#app")}},t={};function r(o){var n=t[o];if(void 0!==n)return n.exports;var a=t[o]={id:o,loaded:!1,exports:{}};return e[o].call(a.exports,a,a.exports,r),a.loaded=!0,a.exports}r.m=e,function(){r.amdO={}}(),function(){var e=[];r.O=function(t,o,n,a){if(!o){var s=1/0;for(c=0;c<e.length;c++){o=e[c][0],n=e[c][1],a=e[c][2];for(var u=!0,l=0;l<o.length;l++)(!1&a||s>=a)&&Object.keys(r.O).every((function(e){return r.O[e](o[l])}))?o.splice(l--,1):(u=!1,a<s&&(s=a));if(u){e.splice(c--,1);var i=n();void 0!==i&&(t=i)}}return t}a=a||0;for(var c=e.length;c>0&&e[c-1][2]>a;c--)e[c]=e[c-1];e[c]=[o,n,a]}}(),function(){r.n=function(e){var t=e&&e.__esModule?function(){return e["default"]}:function(){return e};return r.d(t,{a:t}),t}}(),function(){r.d=function(e,t){for(var o in t)r.o(t,o)&&!r.o(e,o)&&Object.defineProperty(e,o,{enumerable:!0,get:t[o]})}}(),function(){r.g=function(){if("object"===typeof globalThis)return globalThis;try{return this||new Function("return this")()}catch(e){if("object"===typeof window)return window}}()}(),function(){r.hmd=function(e){return e=Object.create(e),e.children||(e.children=[]),Object.defineProperty(e,"exports",{enumerable:!0,set:function(){throw new Error("ES Modules may not assign module.exports or exports.*, Use ESM export syntax, instead: "+e.id)}}),e}}(),function(){r.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)}}(),function(){r.r=function(e){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}}(),function(){r.nmd=function(e){return e.paths=[],e.children||(e.children=[]),e}}(),function(){var e={143:0};r.O.j=function(t){return 0===e[t]};var t=function(t,o){var n,a,s=o[0],u=o[1],l=o[2],i=0;if(s.some((function(t){return 0!==e[t]}))){for(n in u)r.o(u,n)&&(r.m[n]=u[n]);if(l)var c=l(r)}for(t&&t(o);i<s.length;i++)a=s[i],r.o(e,a)&&e[a]&&e[a][0](),e[a]=0;return r.O(c)},o=self["webpackChunkapp_form"]=self["webpackChunkapp_form"]||[];o.forEach(t.bind(null,0)),o.push=t.bind(null,o.push.bind(o))}();var o=r.O(void 0,[998],(function(){return r(5190)}));o=r.O(o)})();
//# sourceMappingURL=app-legacy.js.map