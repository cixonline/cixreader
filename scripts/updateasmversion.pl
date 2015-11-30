# Simple script to update assemblyversion

my $filename = $ARGV[0];
my $version = $ARGV[1];
my $beta = "";

if ($#ARGV == 2) {
    $beta = $ARGV[2];
}

# open body file
open (INFILE, $filename) or die "Couldn't open file: $!";
while (<INFILE>) {
    $body .= $_;
}
close INFILE;

$body =~ s/^\[assembly: AssemblyVersion\(".*"\)\]/\[assembly: AssemblyVersion\("$version"\)\]/m;
$body =~ s/^\[assembly: AssemblyFileVersion\(".*"\)\]/\[assembly: AssemblyFileVersion\("$version"\)\]/m;
$body =~ s/^\[assembly: AssemblyConfiguration\(".*"\)\]/\[assembly: AssemblyConfiguration\("$beta"\)\]/m;

open (OUTFILE, '>', $filename) or die "Couldn't open file: $!";
print OUTFILE $body;
close OUTFILE;