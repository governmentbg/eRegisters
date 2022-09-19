var app = new Vue({ 
      el: "#form",
      components: { 'multiselect' : window.VueMultiselect.default},
      data: function(){
        return { 
		  objectId: '',
          Fields: [] ,
		  SendSuccess: false,
		  SendError: false,
		  LoadError: false
        };
      },

      mounted : function() {
        this.loadJsonData();
      },

      methods : {
        loadJsonData : function () {
          const _this = this;
          $.ajax({
            method: "GET",
            url: getJsonUrl,
            dataType: "json",
            success: function (data){
					_this.Fields = data.Fields;
					_this.objectId = data.Id;
				},
			error: function(){
				   _this.LoadError = true;
			   }
		   });
        },
		sendJsonData : function () {
          const _this = this;
          $.ajax({
            method: "POST",
			dataType: "json",
			/*data: {
				Id: _this.objectId,
				Fields: _this.Fields
			},*/
			data: JSON.stringify({
				Id: _this.objectId,
				Fields: _this.Fields 
			}),
            url: sendJsonUrl,
				success: function (data){
				  _this.SendSuccess = true;
			   },
				error: function(){
				   _this.SendError = true;
			   }
		   });
        }
      }
    });
	
	