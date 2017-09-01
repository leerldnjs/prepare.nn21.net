<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Left_Navigation.ascx.cs" Inherits="ViewShare_Left_Navigation" %>
<section class="w-f scrollable">
	<div class="" data-height="auto" data-disable-fade-out="true" data-distance="0" data-size="10px" data-railopacity="0.2">

		<nav class="nav-primary hidden-xs panel panel-default">
			<header class="panel-heading bg-light">
				<ul class="nav nav-tabs nav-justified">
					<li id="Nav_Work"><a href="#Tab_Work" data-toggle="tab">Work</a></li>
					<li id="Nav_Board"><a href="#Tab_Board" data-toggle="tab">Board</a></li>
				</ul>
			</header>
			<div>
				<div class="tab-content">
					<div class="tab-pane" id="Tab_Work">
						<ul class="nav dk">
							<li style="font-weight:bold;">
								Clearance
							</li>
						</ul>
						<ul class="nav dk">
							<li id="Nav_YuhanView">
								<a href="/Admin/TransportBetweenBranchList.aspx?G=in" class="auto">
									<span>입고현황</span>
								</a>
							</li>
						</ul>
						<ul class="nav dk">
							<li style="font-weight:bold;">
								Finance
							</li>
						</ul>
						<ul class="nav dk">
							<li id="Nav_DebitCredit">
								<a href="/Document/DebitCredit_List.aspx" class="auto">
									<span>DebitCredit</span>
								</a>
							</li>
						</ul>
					</div>
					<div class="tab-pane" id="Tab_Board">
						<ul class="nav dk">
							<li id="Nav_BoardMain">
								<a href="/Board/BoardMain.aspx" class="auto">
									<i class="i i-dot"></i>
									<span>Board</span>
								</a>
							</li>

						</ul>
					</div>
				</div>
			</div>
			<!--
			<footer class="footer hidden-xs no-padder text-center-nav-xs">
				<a href="/Process/Tool/Logout.aspx" class="btn btn-icon icon-muted btn-inactive pull-right m-l-xs m-r-xs hidden-nav-xs">
					<i class="i i-logout"></i>
				</a>
			</footer>
			-->
		</nav>
	</div>
</section>

