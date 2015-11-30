#!/usr/bin/perl -w

use strict;
use Time::Local;
use POSIX;
use POSIX qw(strftime);

# Simple script to update a file and replace tokens

my $infile = $ARGV[0];
my $outfile = $ARGV[1];
my $version = $ARGV[2];
my $folder = $ARGV[3];
my $usescript = $ARGV[4];

# open body file
my $body = '';
my $useline = 1;

open (INFILE, $infile) or die "Couldn't open file: $!";
while (<INFILE>) {
    if ($_ =~ /\$ifscript/) {
		$useline = $usescript;
	}
    elsif ($_ =~ /\$else/) {
		$useline = !$usescript;
	}
    elsif ($_ =~ /\$endif/) {
		$useline = 1;
	}
	else {
		$body .= $_ if $useline == 1;
	}
}
close INFILE;

my $date = strftime("%a, %d %b %Y %H:%M:%S ", localtime(time())) . "+0000";

$body =~ s/\$\(version\)/$version/mg;
$body =~ s/\$\(date\)/$date/mg;
$body =~ s/\$\(folder\)/$folder/mg;

open (OUTFILE, '>', $outfile) or die "Couldn't open file: $!";
print OUTFILE $body;
close OUTFILE;
