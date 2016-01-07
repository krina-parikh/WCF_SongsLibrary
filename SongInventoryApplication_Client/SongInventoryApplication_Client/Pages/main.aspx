<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="SongInventoryApplication_Client.Pages.main" %>

<!DOCTYPE html>

<html lang="en">
  <head>
    <meta charset="utf-8">
    <title>List</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
      <link href="../StyleSheet/song-bootstrap.css" rel="stylesheet" />
      
    <!--[if IE 7]>
      <link rel="stylesheet" href="~/StyleSheet/font-awesome-ie7.min.css">
    <![endif]-->
    <script src="../js/jquery.js"></script>
    <script src="../js/bootstrap.js"></script>
    
    <script>
        $(document).ready(function () {
            if (location.protocol.indexOf('http') > -1) {
                $.getScript('http://www.citigroup.net/search/js/search_rebrand_prod2.js', function () {
                    if (topSearch && topSearch.createSearch) {
                        setTimeout(function () { topSearch.createSearch('top', 'global-search') }, 1000);
                    }
                });
            }
            var megamenuContent = '';
            $('.megamenu ul a').hover(function () {
                megamenuContent = $(this).parents('.megamenu').find('.megamenu-content').html();
                $(this).parents('.megamenu').find('.megamenu-content').css('background-image', 'url(../img/examples/' + $(this).attr('data-bg') + '.jpg)').html('');
            }, function () {
                $(this).parents('.megamenu').find('.megamenu-content').css('background-image', 'url()').html(megamenuContent);
            });
        });
    </script>
  </head>

  <body>
    <div class="utility navbar">
        <div class="navbar-inner">
            <ul class="nav pull-right util">
                
            </ul>
        </div>
        <div class="utility-shadow"></div>
    </div>

	
    <div id="song-header">
        <a href="#" class="logo pull-right" title="" target="_blank">
            
        </a>
        <div class="app_logo">
            <img src="../img/Application-Name.png">
            <!--<h1>Application Name</h1>-->
        </div>
    </div>
    <div id="main-nav" class="navbar">
        <div class="navbar-inner">
        </div>
    </div>
	<div id="body">
		<div id="content" style="padding:36px">
			<div class="row-fluid">
				<div class="span12">
				    <div class="page-header">
					    <h1>Get Album Information</h1> <!-- Get Songs List -->
					</div>
					<div class="row-fluid">
									
						<!-- Floating Modal for Edit information -->
						<div class="modal hide" id="myModal">
							<div class="modal-header">
								<button type="button" class="close" data-dismiss="modal">×</button>
								<h3>Edit Song</h3>
							</div>
							<div class="modal-body">
										<form>
											<fieldset>
<input type="text" id="hiddenRowId" name="hiddenRowId" style="visibility:hidden" hidden="hidden" /><input type="text" hidden="hidden" id="songId" name="songId" style="visibility:hidden" /><input type="text" hidden="hidden" id="albumId" name="albumId" style="visibility:hidden" />
						<div class="control-group">
							<div class="controls">
                                <label for="input01" class="control-label" style="width:190px; float:left"><strong class="required"></strong> Album Title</label>
							
								<input type="text" id="albumTitle" class="input-large" disabled="disabled">
							</div>
						</div>
						<div class="control-group">
							<label for="input01" class="control-label" style="width:190px; float:left"><strong class="required"></strong> Song Title</label>
							<div class="controls">
								<input type="text" id="songTitle" class="input-large">
							</div>
						</div>
						<div class="control-group">
							<label for="input01" class="control-label" style="width:190px; float:left"><strong class="required"></strong> Length</label>
							<div class="controls">
								<input type="text" class="input-large" id="songLength">
							</div>
						</div>
						<div class="control-group">
							<label for="select01" class="control-label" style="width:190px; float:left">Track Number</label>
							<div class="controls">
								<input type="text" class="input-large" id="trackNumber">
							</div>
						</div>
						<div class="control-group">
							<label for="multiSelect" class="control-label" style="width:190px; float:left">Genre</label>
							<div class="controls">
								<input type="text" class="input-large" id="genre" value="">
							</div>
						</div>
					</fieldset>
										</form>
									</div>
							<div class="modal-footer">
								<a href="#" class="btn" data-dismiss="modal">Close</a>
								<a href="#" class="btn btn-primary" id="btnUpdate">Save changes</a>
							</div>
						</div>
						<!-- End of - Floating Modal for Edit information -->		
					</div>		
					
					<form class="form-inline actions-toolbar">
						<div class="actions-toolbar-inner">
							<label>Search Value:</label>
							<input type="text" id="searchTxt" value="" class="input-medium" style="margin-right:5px;">
							
							<button id="btnFilter" class="btn btn-primary"><i class="icon-filter icon-white"></i>&nbsp;Filter</button>
							<button class="btn"><i class="icon-remove"></i>&nbsp;Clear</button>
						</div>
					</form>	
                    <div id="addNew" class="actions-toolbar-inner" style="padding-left:0px; padding-bottom:10px; display:none">
                        <form class="form-horizontal form-container">
                            <fieldset style="padding:10px 10px 10px">
                                <div class="page-header">
                                    <div class="control-group actions-toolbar-inner" style="width:1405px;float:left;">
                                        <label class="control-label" style="width:50px;text-align:left;font-size:15px"><strong>Album</strong></label>
                                        <div style="margin-left:15px">
                                            <label class="control-label" id="lblAlbumName" style="width:100px;text-align:left"> </label>
                                        </div>
                                        <label class="control-label" style="width:50px;text-align:left;font-size:15px"><strong>Artist</strong></label>
                                        <div style="margin-left: 15px">
                                            <label class="control-label" id="lblArtist" style="width: 100px;text-align:left"></label>
                                        </div>
                                    </div>
                                    
                                </div>
                                <div class="control-group actions-toolbar-inner" style="float:left">
                                    <label class="control-label" style="width:100px">Song Title</label>
                                    <div class="controls" style="margin-left:115px">
                                        <input type="text" id="newSongTitle" class="input-large" />
                                    </div>
                                </div>
                                <div class=" control-group actions-toolbar-inner" style="float:left">
                                    <label class="control-label" style="width:100px">Length</label>
                                    <div class="controls" style="margin-left:115px">
                                        <input type="text" class="input-large" id="newLength" />
                                    </div>
                                </div>
                                <div class="control-group actions-toolbar-inner" style="float: left">
                                    <label class="control-label" style="width:100px">Track Number</label>
                                    <div class="controls" style="margin-left:115px">
                                        <input type="text" class="input-large" id="newTrackNumber" />
                                    </div>
                                </div>
                                <div class="control-group actions-toolbar-inner" style="width:inherit; float:left">
                                    <label class="control-label" style="width:100px">Genre</label>
                                    <div class="controls" style="margin-left:115px">
                                        <input type="text" class="input-large" id="newGenre" />
                                    </div>
                                </div>
                                <div class="btn-group control-group" style="padding-left:5px">
                                    <button class="btn btn-primary" id="btnAddSong" data-target="#addModal" data-toggle="modal"><i class="icon-plus"></i>&nbsp;Add Song</button>
                                </div>
                            </fieldset>

                        </form>    
                        </div>
                    <div class="page-header">
					    <label style="font-size:18px; line-height:24px; font-weight:lighter;color:#002D72;">Record Set</label>
					</div>
					<table id="example" class="table table-bordered table-striped">
						<thead>
							<tr>
								<th>Id<i class="sort"></i></th>
								<th>Song Title<i class="sort"></i></th>
								<th>Length<i class="sort"></i></th>
								<th>TrackNumber<i class="sort"></i></th>
								<th>Genre<i class="sort"></i></th>
								<th>Edit<i class="sort"></i></th>
							</tr>
						</thead>
					</table>
					<div class="row-fluid actions-toolbar">
						
					</div>				
				</div>
			</div>
	</div><!--/.fluid-container-->

	<style>
    table.table thead .sorting,
    table.table thead .sorting_asc,
    table.table thead .sorting_desc,
    table.table thead .sorting_asc_disabled,
    table.table thead .sorting_desc_disabled {
        cursor: pointer;
        *cursor: hand;
    }
    .dataTables_wrapper .row {
        display:none;
    }
    </style>
    <script src="../js/jquery.dataTables.js"></script>
    <script src="../js/DT_bootstrap.js"></script>
    <script>
        
        /* Table initialisation */
        $(document).ready(function () {
            $("#example").DataTable({
                "sDom": "<'row'<'span6'l><'span6'f>r>t<'row'<'span6'i><'span6'p>>",
                "sPaginationType": "bootstrap",
                "oLanguage": {
                    "sLengthMenu": "_MENU_ records per page"
                },
                "aaSorting": [],
                "iDisplayLength": 10000,
                "aoColumnDefs": [
                  { "bSortable": false, "aTargets": [1] }
                ]
            });
            
        });
        $("#myModal").on("show.bs.modal", function (event) {
            var id = $("#hiddenRowId").val();
            var data = id.split('+');
            if (data.length == 7) {
                $("#albumId").val(data[0]);
                $("#songId").val(data[1]);
                $("#albumTitle").val(data[2]);
                $("#songTitle").val(data[3]);
                $("#songLength").val(data[4]);
                $("#trackNumber").val(data[5]);
                $("#genre").val(data[6]);
            }
        });
        $("#btnUpdate").click(function () {
            var dt = getDataTableObject();
            var data = [];
            data.push($("#albumId").val());
            data.push($("#songId").val());
            data.push($("#albumTitle").val());
            data.push($("#songTitle").val());
            data.push($("#songLength").val());
            data.push($("#trackNumber").val());
            data.push($("#genre").val());
            
            var rowIndx = $("#" + data[0] + '-' + data[1]).index();
            if (rowIndx > 0) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify({ "albumId": data[0], "songId": data[1], "albumName": data[2], "songName": data[3], "length": data[4], "trackNumber": data[5], "genre": data[6] }),
                    url: "main.aspx/UpdateSongData",
                    success: function (data) {
                        dt.fnUpdate([data[3]], rowIndx, 1);
                        dt.fnUpdate([data[4]], rowIndx, 2);
                        dt.fnUpdate([data[5]], rowIndx, 3);
                        dt.fnUpdate([data[6]], rowIndx, 4);
                        $("#btnUpdate").prop('disabled', true);
                    },
                    error: function (failure) {
                        //alert('Failure' + failure);
                    }
                });
            }
        });
        function btnEdit(e) {
            var rowid = $(e).attr('id');
            $('#hiddenRowId').val(rowid);
            $("#btnUpdate").prop('disabled', false);
            return false;
        };
        $("#btnAddSong").click(function () {
            var dt = getDataTableObject();
            var data = [];
            data.push($("#albumId").val());
            data.push($("#searchTxt").val());
            data.push($("#newSongTitle").val());
            data.push($("#newLength").val());
            data.push($("#newTrackNumber").val());
            data.push($("#newGenre").val());

            $.ajax({
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({"albumId": data[0], "albumName": data[1], "songName": data[2], "length": data[3], "trackNumber": data[4], "genre": data[5] }),
                url: "main.aspx/AddSongRecord",
                success: function (result) {
                    var items = this.d;
                    $.each(result.d, function (key, val) {
                        alert(key + "-" + val.toString());
                        if (val.toString() != "") {
                            var editID = data[0] + '+' + val.toString() + '+' + data[1] + '+' + data[2] + '+' + data[3] + '+' + data[4] + '+' + data[5];
                            var nodeId = dt.fnAddData(['<tr><td id="' + val.SongId + '">' + parseInt(val.toString(), 10) + '</td>',
                                                        '<td>' + data[2] + '</td>',
                                                        '<td>' + data[3] + '</td>',
                                                        '<td>' + data[4] + '</td>',
                                                        '<td>' + data[5] + '</td>',
                                                        '<td align="center"><a data-toggle="modal" data-target="#myModal" onclick="btnEdit(this)" id= "' + editID +
                                                        '"><i class="icon-pencil"></i></a></td>'
                                                        //,'<td align="center"><a href="#" title="Delete" onclick="btnDeleteRecord(this)"><i class="icon-remove"></i></a></td></tr>'
                            ]);
                            var node = dt.fnSettings().aoData[nodeId[0]].nTr;
                            node.setAttribute('id', data[0] + '-' + val.toString());
                        }
                    });
                    $("#addNew").css("display", "");
                    $("#albumId").val('');
                    $("#searchTxt").val('');
                    $("#newSongTitle").val('');
                    $("#newLength").val('');
                    $("#newTrackNumber").val('');
                    $("#newGenre").val('');
                },
                error: function (failure) {
                    alert(failure.responseText);
                }
            });
        });
        $("#btnFilter").click(function () {
            var albumName = $("#searchTxt").val();
            var dt = getDataTableObject(); 
            
            $.ajax({
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({ "albumName": albumName }),
                url: "main.aspx/GetAlbumData",
                success: function (data) {
                    var items = this.d;
                    dt.fnClearTable();
                    $.each(data.d, function (key, val) {
                        var editID = val.AlbumId + '+' + val.SongId + '+' + val.AlbumName + '+' + val.SongName + '+' + val.Length + '+' + val.TrackNumber + '+' + val.Genre;
                        var nodeId = dt.fnAddData(['<tr><td>' + parseInt(val.SongId, 10) + '</td>',
                                                    '<td>' + val.SongName + '</td>',
                                                    '<td>' + val.Length + '</td>',
                                                    '<td>' + val.TrackNumber + '</td>',
                                                    '<td>' + val.Genre + '</td>',
                                                    '<td align="center"><a data-toggle="modal" data-target="#myModal" onclick="btnEdit(this)" id= "' + editID +
                                                    '"><i class="icon-pencil"></i></a></td>'
                                                    //,'<td align="center"><a href="#" title="Delete" onclick="btnDeleteRecord(this)"><i class="icon-remove"></i></a></td></tr>'
                                                ]);
                        var node = dt.fnSettings().aoData[nodeId[0]].nTr;
                        node.setAttribute('id', val.AlbumId + '-' + val.SongId);
                        $("#albumId").val(val.AlbumId);
                        $("#lblArtist").html(val.Artist);
                    });
                    $("#addNew").css("display", "");
                    $("#lblAlbumName").html(albumName);
                },
                error: function (failure) {
                    alert(failure.responseText);
                }
            });
            return false;
        });
        function btnDeleteRecord(element) {
            var dt = getDataTableObject();
            var row = $(element).closest('tr').attr('id');
            alert(row);
        };
        function getDataTableObject(){
            return $("#example").DataTable();
        }
    </script>
</div>
	</div>
	<div class="clearfix"></div>
    <div id="footer">
        <div class="navbar">
            <div class="navbar-inner">
                <a href="#" class="pull-right">Back to top</a>
                <div class="nav-collapse">
                    <ul class="nav">
                        <li><a href="#" target="_blank">Terms &amp; Conditions</a></li>
                        <li><a href="#" target="_blank">Accessibility</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div id="copyright">					
			<span class="pull-right copyright">&copy; 2012 Songs Inc.</span>
        </div>
    </div>
  </body>
</html>

